package servlet;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.PrintWriter;
import java.util.ArrayList;

import javax.servlet.ServletConfig;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import model.DataManager;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import bean.messageInfo;
import bean.productInfo;
import bean.userInfo;

public class servlet extends HttpServlet {
	private static final long serialVersionUID = 1L;
	public static final int maxDice = 5;

	private DataManager dataManager;
	private double dTime;

	public servlet() {
		super();
	}

	public void init(ServletConfig config) throws ServletException {
		super.init();
		dataManager = new DataManager();
		dataManager.setDbUrl(config.getInitParameter("dbUrl"));
		dataManager.setDbUser(config.getInitParameter("dbUser"));
		dataManager.setDbPass(config.getInitParameter("dbPass"));

		try {
			Class.forName(config.getInitParameter("jdbcDriver"));
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	protected void doGet(HttpServletRequest request,
			HttpServletResponse response) throws ServletException, IOException {
		try {
			doProcess(request, response);
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

	protected void doPost(HttpServletRequest request,
			HttpServletResponse response) throws ServletException, IOException {
		try {
			doProcess(request, response);
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

	protected void doProcess(HttpServletRequest request,
			HttpServletResponse response) throws ServletException, IOException,
			JSONException {
		String jsonObjectString = "";
		BufferedReader br = request.getReader();
		if (br == null) {
			System.out.println("buffer is null");
			return;
		}

		// System.out.println((new JSONObject(request)).toString());

		String temp;
		while ((temp = br.readLine()) != null) {
			jsonObjectString += temp;
		}

		if (jsonObjectString.equals("")) {
			return;
		}

		JSONObject object = new JSONObject(jsonObjectString);
		if (object == null) {
			System.out.println("object is null");
			return;
		}

		if (object.opt("object") != null) {
			// payment
			System.out.println("payment process");
			PaymentProcess(object, response);
		} else if (object.opt("option") != null) {
			// server process
			System.out.println("server process");
			ServerProcess(object, response);
		}
	}

	public void PaymentProcess(JSONObject object, HttpServletResponse response)
			throws ServletException, IOException, JSONException {
		JSONArray jsonArray = object.getJSONArray("entry");
		String id = (String) jsonArray.getJSONObject(0).get("id");
		productInfo product = urlConnection.sendToOpenGraphPayment(id);

		if (product == null)
			return;

		int nGold = dataManager.getGold(product.getFid());
		if (product.getName().equals("http://thepipercat.com/money01.html") == true) {
			// 10
			dataManager.setGold(product.getFid(), nGold + 10);
		} else if (product.getName().equals(
				"http://thepipercat.com/money02.html") == true) {
			// 50
			dataManager.setGold(product.getFid(), nGold + 50);
		} else if (product.getName().equals(
				"http://thepipercat.com/money03.html") == true) {
			// 100
			dataManager.setGold(product.getFid(), nGold + 100);
		} else if (product.getName().equals(
				"http://thepipercat.com/money04.html") == true) {
			// 500
			dataManager.setGold(product.getFid(), nGold + 500);
		} else if (product.getName().equals(
				"http://thepipercat.com/money05.html") == true) {
			// 1000
			dataManager.setGold(product.getFid(), nGold + 1000);
		} else if (product.getName().equals("http://thepipercat.com/test.html") == true) {
			// 1000
			dataManager.setGold(product.getFid(), nGold + 1000);
		}
	}

	public void ServerProcess(JSONObject object, HttpServletResponse response)
			throws ServletException, IOException, JSONException {
		String option = (String) object.get("option");
		System.out.println(option);

		if (option.equals("login")) {
			JSONObject obj = new JSONObject();
			String fid = (String) object.get("fid");
			System.out.println("fid = " + fid);
			String name = "";

			Object objectName = object.get("name");

			if (objectName != null) {
				name = (String) objectName;
			}

			// 존재하는 user인지 검사
			if (dataManager.IsinUser(fid) == false) {
				userInfo user = new userInfo();
				user.setFid(fid);
				user.setName(name);
				user.setDice(Integer.toString(maxDice));
				user.setGold("0");
				user.setScore("0");
				user.setTutorial("0");
				dataManager.addUser(user);
			}

			// gold
			int nGold = dataManager.getGold(fid);
			obj.put("gold", Integer.toString(nGold));

			// dice
			int nDice = dataManager.getDice(fid);

			int nDicePluseTime = 1800;
			int nTime = dataManager.getTime(fid);
			int nCurrentTime = dataManager.getCurrentTime();
			int nDuration = nCurrentTime - nTime;
			int nAddDice = nDuration / nDicePluseTime;
			int nDiceTime = nDuration % nDicePluseTime;

			if (nDice == maxDice) {
				nDiceTime = -1;
			} else {
				if (nAddDice > 0) {
					if (nAddDice + nDice < maxDice) {
						// dice 추가양이 maxdice 보다 작을경우
						nDice += nAddDice;
						dataManager.setDice(fid, nDice, nCurrentTime);
						nDiceTime = nDuration % nDicePluseTime;
					} else {
						// dice 추가양이 maxdice 보다 클경우 maxdice로 세팅
						nDice = maxDice;
						dataManager.setDice(fid, nDice, nCurrentTime);
						nDiceTime = -1;
					}
				}
			}

			// dice
			obj.put("dice", Integer.toString(nDice));

			// dice time
			obj.put("dicetime", Integer.toString(nDiceTime));

			// score
			int nScore = dataManager.getScore(fid);
			obj.put("score", Integer.toString(nScore));

			// top
			ArrayList<userInfo> userList = dataManager.getTop();
			obj.put("top", userList);

			// ranking
			int nRanking = dataManager.getRankingByID(fid);
			obj.put("ranking", Integer.toString(nRanking));
			System.out.println("ranking = " + nRanking);

			// tutorial
			int tutorial = dataManager.getTutorial(fid);
			obj.put("tutorial", Integer.toString(tutorial));
			System.out.println("tutorial = " + tutorial);

			// achive
			obj.put("achive", 0);

			// item
			obj.put("item", 0);

			// message
			obj.put("message", 0);

			response.setCharacterEncoding("UTF-8");
			PrintWriter writer = response.getWriter();
			writer.write(obj.toString());
			return;
		} else if (option.equals("game_start")) {
			JSONObject obj = new JSONObject();
			String fid = (String) object.get("fid");
			System.out.println("fid = " + fid);

			int nDice = dataManager.getDice(fid);
			if (nDice > 0) {
				// 게임 시작
				// idce 1 감소
				nDice = nDice - 1;
				dataManager.setDice(fid, nDice, dataManager.getCurrentTime());
				obj.put("result", 1);

			} else {
				// 게임 시작 불가
				obj.put("result", 0);
			}

			response.setCharacterEncoding("UTF-8");
			PrintWriter writer = response.getWriter();
			writer.write(obj.toString());
		} else if (option.equals("game_end")) {
			JSONObject obj = new JSONObject();
			String fid = (String) object.get("fid");
			System.out.println("fid = " + fid);
			String score = (String) object.get("score");
			System.out.println("score = " + score);

			dataManager.setScore(fid, score);
			obj.put("result", 1);
			response.setCharacterEncoding("UTF-8");
			PrintWriter writer = response.getWriter();
			writer.write(obj.toString());
		} else if (option.equals("tutorial_end")) {
			JSONObject obj = new JSONObject();
			String fid = (String) object.get("fid");
			System.out.println("fid = " + fid);

			dataManager.setTutorial(fid, "1");
			obj.put("result", 1);
			response.setCharacterEncoding("UTF-8");
			PrintWriter writer = response.getWriter();
			writer.write(obj.toString());
		} else if (option.equals("message_list")) {

			JSONObject obj = new JSONObject();
			String receiver_fid = (String) object.get("receiver_fid");
			System.out.println("receiver_fid = " + receiver_fid);
			ArrayList<messageInfo> messageList = dataManager
					.getMessageList(receiver_fid);
			obj.put("message_list", messageList);
			response.setCharacterEncoding("UTF-8");
			PrintWriter writer = response.getWriter();
			writer.write(obj.toString());
		} else if (option.equals("send_message")) {

			JSONObject obj = new JSONObject();
			String sender_fid = (String) object.get("sender_fid");
			System.out.println("sender_fid = " + sender_fid);

			String receiver_fid = (String) object.get("receiver_fid");
			System.out.println("receiver_fid = " + receiver_fid);

			String message_type = (String) object.get("message_type");
			System.out.println("message_type = " + message_type);

			messageInfo message = new messageInfo();
			message.setSenderFid(sender_fid);
			message.setReceiveFid(receiver_fid);
			message.setType(message_type);
			dataManager.addMessage(message, dataManager.getCurrentTime());

			obj.put("result", 1);
			response.setCharacterEncoding("UTF-8");
			PrintWriter writer = response.getWriter();
			writer.write(obj.toString());

		} else if (option.equals("send_message_list")) {

			JSONObject obj = new JSONObject();
			String sender_fid = (String) object.get("sender_fid");
			System.out.println("sender_fid = " + sender_fid);

			String message_type = (String) object.get("message_type");
			System.out.println("message_type = " + message_type);

			String receiver_fid = (String) object.get("receiver_fid");
			String[] receiverArray = receiver_fid.split(",");
			for (String receiverFid : receiverArray) {
				System.out.println("receiver_fid = " + receiverFid);
				messageInfo message = new messageInfo();
				message.setSenderFid(sender_fid);
				message.setReceiveFid(receiverFid);
				message.setType(message_type);
				dataManager.addMessage(message, dataManager.getCurrentTime());
			}

			obj.put("result", 1);
			response.setCharacterEncoding("UTF-8");
			PrintWriter writer = response.getWriter();
			writer.write(obj.toString());

		} else if (option.equals("friend_ranking")) {
			JSONObject obj = new JSONObject();
			String sender_fid = (String) object.get("sender_fid");
			System.out.println("sender_fid = " + sender_fid);
			int reqScore = Integer.parseInt((String) object.get("score"));
			String friend_list = (String) object.get("friend_list");
			String[] friendArray = friend_list.split(",");
			userInfo[] userList = new userInfo[8];
		
			int nCurrentTime = dataManager.getCurrentTime();
			int nDay = 60 * 60 * 24;
			
			//이전 랭킹과 현재 점수로 예상되는 랭킹을 비교하여 랭킹의 변화를 측정 한다.
			int nRankingPrev = dataManager.getRankingByIDFromFriend(sender_fid,
					friendArray);
			int nRankingCurrent = dataManager.getRankingByScoreFromFriend(
					reqScore, sender_fid, friendArray);
			boolean bChange = nRankingCurrent < nRankingPrev;

			// database의 기록을 갱신 시키는 부분
			if (reqScore > dataManager.getScore(sender_fid)) {
				dataManager.setScore(sender_fid, Integer.toString(reqScore));
			}

			// 기록 갱신된 새로운 점수를 구한다.
			nRankingCurrent = dataManager.getRankingByIDFromFriend(sender_fid,
					friendArray);
			
			int nTotalRank = dataManager.getTotalRank(friendArray, sender_fid);
			obj.put("ranking", Integer.toString(nRankingCurrent));
			
			//상위 6등 까지의 기록을 얻어온다.
			dataManager.getTopFromFriend(userList,friendArray, 6, sender_fid);

			if (nRankingCurrent == nTotalRank) {
				// 꼴등일경우 아래 사람이 없기 때문에 위에 2명
				ArrayList<userInfo> upUserList = dataManager
						.getUpFromFriendByID(friendArray, sender_fid, 2);

				if (upUserList.size() > 0) {
					userList[6] = upUserList.get(0);
				}

				if (upUserList.size() > 1) {
					userList[7] = upUserList.get(1);
				}
				
			} else {
				//그렇지 않을경우 위에 1명 아래 1명 추가
				ArrayList<userInfo> upUserList = dataManager
						.getUpFromFriendByID(friendArray, sender_fid, 1);
				ArrayList<userInfo> downUserList = dataManager
						.getDownFromFriendByID(friendArray, sender_fid, 1);

				if (upUserList.size() > 0) {
					userList[6] = upUserList.get(0);
				}

				if (downUserList.size() > 0) {
					userList[7] = downUserList.get(0);
				}
			}

			if (bChange == true) {
				obj.put("change", "1");
			} else {
				obj.put("change", "0");
			}

			for(int i=0;i<8;i++)
			{
				userInfo user = userList[i];
				
				if(user == null)
				{
					obj.put(i + "_fid", -1);
					continue;
				}
				
				obj.put(i + "_fid", user.getFid());
				obj.put(i + "_name", user.getName());

				int nTime = dataManager.getMessageTime(sender_fid,
						user.getFid());				
				if (nCurrentTime - nTime > nDay) {
					obj.put(i + "_can_send", "1");
				} else {
					obj.put(i + "_can_send", "0");
				}

				obj.put(i + "_score", user.getScore());				
			}			

			response.setCharacterEncoding("UTF-8");
			PrintWriter writer = response.getWriter();
			writer.write(obj.toString());

		} else if (option.equals("receive_message")) {

			JSONObject obj = new JSONObject();
			String sender_fid = (String) object.get("sender_fid");
			System.out.println("sender_fid = " + sender_fid);

			String receiver_fid = (String) object.get("receiver_fid");
			System.out.println("receiver_fid = " + receiver_fid);

			String message_type = (String) object.get("message_type");
			System.out.println("message_type = " + message_type);

			String sub_option = (String) object.get("sub_option");
			System.out.println("message_type = " + message_type);

			messageInfo message = new messageInfo();
			message.setSenderFid(sender_fid);
			message.setReceiveFid(receiver_fid);
			message.setType(message_type);

			int nCurrentTime = dataManager.getCurrentTime();
			if (dataManager.delMessage(message)) {
				// 메세지를 정상 적으로 지울 경우
				if (sub_option.equals("accept")) {
					// 메세지를 수락 할경우
					switch (Integer.parseInt(message_type)) {
					case 1:
					case 2:
						// 주사위 추가
						int nDice = dataManager.getDice(receiver_fid);

						// max dice보다 작을경우 1개 추가
						if (nDice < maxDice)
							nDice += 1;
						else
							nDice = maxDice;

						dataManager.setDice(receiver_fid, nDice, nCurrentTime);
						break;
					case 3:
						// sender에게 메세지 추가
						messageInfo newMessage = new messageInfo();

						newMessage.setReceiveFid(message.getSenderFid());
						newMessage.setSenderFid(message.getReceiveFid());
						newMessage.setType("2");

						dataManager.addMessage(newMessage,
								dataManager.getCurrentTime());
						break;
					}
				} else if (sub_option.equals("cancel")) {
					// 수락 하지 않고 지울 경우
				}
			}

			obj.put("result", dataManager.getDice(receiver_fid));
			response.setCharacterEncoding("UTF-8");
			PrintWriter writer = response.getWriter();
			writer.write(obj.toString());

		} else if (option.equals("message_check")) {
			JSONObject obj = new JSONObject();
			String sender_fid = (String) object.get("sender_fid");
			System.out.println("sender_fid = " + sender_fid);

			String friend_list = (String) object.get("friend_list");
			String[] friendArray = friend_list.split(",");
			ArrayList<String> resultArray = new ArrayList<String>();
			int nCurrentTime = dataManager.getCurrentTime();
			for (String friendFid : friendArray) {

				// 친구 정보를 읽고 시간 검사
				int nTime = dataManager.getMessageTime(sender_fid, friendFid);
				// 메세지 보낸 시간과 현재 시간 차이가 24 시간 이상일 경우
				int nDay = 60 * 60 * 24;
				if (nCurrentTime - nTime > nDay) {
					resultArray.add("1");
				} else {
					resultArray.add("0");
				}
			}

			obj.put("result", resultArray);
			response.setCharacterEncoding("UTF-8");
			PrintWriter writer = response.getWriter();
			writer.write(obj.toString());
			
		} else if (option.equals("buy_dice")) {
			JSONObject obj = new JSONObject();
			String fid = (String) object.get("fid");
			System.out.println("fid = " + fid);

			int nCurrentTime = dataManager.getCurrentTime();
			int nDice = dataManager.getDice(fid);
			int nTime = dataManager.getTime(fid);
			int nDiceTime = nCurrentTime - nTime;
			int nGold = dataManager.getGold(fid);

			if (nGold > 12) {
				// dice 결제 max로 세팅
				dataManager.setGold(fid, nGold - 12);
				nDice = maxDice;
				dataManager.setDice(fid, nDice, nCurrentTime);
			}

			nGold = dataManager.getGold(fid);
			obj.put("gold", Integer.toString(nGold));
			obj.put("dice", nDice);
			obj.put("dicetime", nDiceTime);

			response.setCharacterEncoding("UTF-8");
			PrintWriter writer = response.getWriter();
			writer.write(obj.toString());
		} else if (option.equals("dicepluse")) {

			JSONObject obj = new JSONObject();
			String fid = (String) object.get("fid");
			System.out.println("fid = " + fid);

			int nCurrentTime = dataManager.getCurrentTime();
			int nDice = dataManager.getDice(fid);
			int nTime = dataManager.getTime(fid);
			int nDiceTime = nCurrentTime - nTime;
			System.out.println("dice pluse = " + nDiceTime);

			if (nDiceTime > 1790) {
				if (nDice < maxDice) {
					nDice += 1;
				} else {
					nDice = maxDice;
					nDiceTime = -1;
				}

				System.out.println("current time = " + nCurrentTime);
				System.out.println("dice = " + nDice);
				dataManager.setDice(fid, nDice, nCurrentTime);
			}

			// 남은 시간 돌려 줌
			obj.put("dice", nDice);
			obj.put("dicetime", nDiceTime);
			response.setCharacterEncoding("UTF-8");
			PrintWriter writer = response.getWriter();
			writer.write(obj.toString());
		}
	}
}