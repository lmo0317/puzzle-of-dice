package model;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;

import bean.messageInfo;
import bean.userInfo;

public class DataManager {
	private String dburl = "";
	private String dbuser = "";
	private String dbpass = "";

	public void setDbUrl(String url) {
		dburl = url;
	}

	public String getDbUrl() {
		return dburl;
	}

	public void setDbUser(String user) {
		dbuser = user;
	}

	public String getDbUser() {
		return dbuser;
	}

	public void setDbPass(String pass) {
		dbpass = pass;
	}

	public String getDbPass() {
		return dbpass;
	}

	public Connection getConnection() {
		Connection conn = null;
		try {
			conn = DriverManager.getConnection(getDbUrl(), getDbUser(),
					getDbPass());
		} catch (SQLException e) {
			e.printStackTrace();
		}
		return conn;
	}

	public void closeConnection(Connection conn) {
		if (conn != null) {
			try {
				conn.close();
			} catch (SQLException e) {
				e.printStackTrace();
			}
		}
	}

	public ArrayList<userInfo> getTop() {
		ArrayList<userInfo> userList = new ArrayList<userInfo>();
		Connection conn = getConnection();

		if (conn != null) {
			try {
				Statement st = conn.createStatement();
				String sqlQuery = "SELECT * FROM score_table order by score desc limit 10";
				ResultSet rs = st.executeQuery(sqlQuery);

				while (rs.next()) {
					userInfo user = new userInfo();
					user.setFid(rs.getString("id"));
					user.setScore(rs.getString("score"));
					user.setName(rs.getString("name"));
					userList.add(user);
				}
			} catch (SQLException e) {
				// e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}

		return userList;
	}
	
	public int getTotalRank(String[] friendList, String fid)
	{
		Connection conn = getConnection();
		if (conn != null) {
			try {
				
				Statement st = conn.createStatement();
				int nScore = getScore(fid);
				String sqlQuery = "SELECT COUNT(*) FROM score_table WHERE ";

				sqlQuery += "id = " + "'" + fid + "'";
				for (int i = 0; i < friendList.length; ++i) {
					String friendFid = friendList[i];
					if(friendFid.equals("") == false) {
						sqlQuery += " or " + "id = " + "'" + friendFid + "'";
					}
				}
				
				System.out.println("getTotalRank Query = " + sqlQuery);
				ResultSet rs = st.executeQuery(sqlQuery);
				if (rs.next()) {
					return Integer.parseInt(rs.getString(1));
				}
			} catch (SQLException e) {
				e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}
		
		return 0;
	}
	
	public ArrayList<userInfo> getUpFromFriendByID(String[] friendList, String fid,int nCount) {
	
		ArrayList<userInfo> userList = new ArrayList<userInfo>();
		Connection conn = getConnection();
		int nScore = getScore(fid);
		
		if (conn != null) {
			try {
				Statement st = conn.createStatement();
				String sqlQuery = "SELECT * FROM score_table ";
				sqlQuery += "where score > " + "'" + nScore + "'" + " AND (";

				sqlQuery += "id = " + "'" + fid + "'";
				for (int i = 0; i < friendList.length; ++i) {
					String friendFid = friendList[i];
					if(friendFid.equals("") == false) {
						sqlQuery += " or " + "id = " + "'" + friendFid + "'";
					}
				}
				
				sqlQuery += " ) ";
				sqlQuery += " order by score asc limit " + nCount;
				System.out.println("getUpFromFriendByID Query = " + sqlQuery);
				ResultSet rs = st.executeQuery(sqlQuery);

				while (rs.next()) {
					userInfo user = new userInfo();
					user.setFid(rs.getString("id"));
					user.setScore(rs.getString("score"));
					user.setName(rs.getString("name"));
					userList.add(user);
				}
			} catch (SQLException e) {
				e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}

		return userList;
	}

	public ArrayList<userInfo> getDownFromFriendByID(String[] friendList, String fid,int nCount) {
		ArrayList<userInfo> userList = new ArrayList<userInfo>();
		Connection conn = getConnection();
		int nScore = getScore(fid);
		
		if (conn != null) {
			try {
				Statement st = conn.createStatement();
				String sqlQuery = "SELECT * FROM score_table ";
				sqlQuery += "where score < " + "'" + nScore + "'" + "AND (";

				sqlQuery += "id = " + "'" + fid + "'";
				for (int i = 0; i < friendList.length; ++i) {
					String friendFid = friendList[i];
					
					if(friendFid.equals("") == false) {
						sqlQuery +=  " or " + "id = " + "'" + friendFid + "'";
					}
				}

				sqlQuery += " ) ";
				sqlQuery += " order by score desc limit " + nCount;
				
				System.out.println("getDownFromFriendByID Query = " + sqlQuery);
				ResultSet rs = st.executeQuery(sqlQuery);

				while (rs.next()) {
					userInfo user = new userInfo();
					user.setFid(rs.getString("id"));
					user.setScore(rs.getString("score"));
					user.setName(rs.getString("name"));
					userList.add(user);
				}
			} catch (SQLException e) {
				e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}

		return userList;
	}

	public void getTopFromFriend(userInfo[] userList,String[] friendList, int nUserCount ,String fid) {
		Connection conn = getConnection();

		if (conn != null) {
			try {
				Statement st = conn.createStatement();
				String sqlQuery = "SELECT * FROM score_table where ";

				sqlQuery += "id = " + "'" + fid + "'";
				for (int i = 0; i < friendList.length; ++i) {
					String friendFid = friendList[i];
					
					if(friendFid.equals("") == false) {
						sqlQuery +=  " or " + "id = " + "'" + friendFid + "'";
					}
				}

				sqlQuery += " order by score desc limit " + nUserCount;

				System.out.println("getTopFromFriend Query = " + sqlQuery);
				ResultSet rs = st.executeQuery(sqlQuery);

				int nCount = 0;
				while (rs.next()) {
					userInfo user = new userInfo();
					user.setFid(rs.getString("id"));
					user.setScore(rs.getString("score"));
					user.setName(rs.getString("name"));
					userList[nCount++] = user;
				}
			} catch (SQLException e) {
				// e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}
	}

	public int getRankingByID(String fid) {
		Connection conn = getConnection();
		if (conn != null) {
			try {
				Statement st = conn.createStatement();
				int nScore = getScore(fid);
				String sqlQuery = "SELECT COUNT(*) + 1 FROM score_table WHERE score > "
						+ "'" + nScore + "'" + ";";
				ResultSet rs = st.executeQuery(sqlQuery);

				if (rs.next()) {
					return Integer.parseInt(rs.getString(1));
				}

			} catch (SQLException e) {
				// e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}
		return 0;
	}

	public int getRankingByScore(int nScore) {
		Connection conn = getConnection();
		if (conn != null) {
			try {
				Statement st = conn.createStatement();
				// int nScore = getScore(fid);
				String sqlQuery = "SELECT COUNT(*) + 1 FROM score_table WHERE score > "
						+ "'" + nScore + "'" + ";";
				ResultSet rs = st.executeQuery(sqlQuery);

				if (rs.next()) {
					return Integer.parseInt(rs.getString(1));
				}

			} catch (SQLException e) {
				// e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}
		return 0;
	}

	public int getRankingByIDFromFriend(String fid, String[] friendList) {
		Connection conn = getConnection();
		if (conn != null) {
			try {
				Statement st = conn.createStatement();
				int nScore = getScore(fid);
				String sqlQuery = "SELECT COUNT(*) + 1 FROM score_table WHERE score > "
						+ "'" + nScore + "'" + " AND (";

				sqlQuery += "id = " + "'" + fid + "'";
				for (int i = 0; i < friendList.length; ++i) {
					String friendFid = friendList[i];
					if(friendFid.equals("") == false) {
						sqlQuery += " or " + "id = " + "'" + friendFid + "'";
					}
				}

				sqlQuery += " ) ";
				System.out.println("getRankingByIDFromFirend Query = " + sqlQuery);
				ResultSet rs = st.executeQuery(sqlQuery);

				if (rs.next()) {
					return Integer.parseInt(rs.getString(1));
				}

			} catch (SQLException e) {
				// e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}
		return 0;
	}

	public int getRankingByScoreFromFriend(int nScore,String fid, String[] friendList) {
		Connection conn = getConnection();
		if (conn != null) {
			try {
				Statement st = conn.createStatement();
				String sqlQuery = "SELECT COUNT(*) + 1 FROM score_table WHERE score > "
						+ "'" + nScore + "'" + " AND (";

				sqlQuery += "id = " + "'" + fid + "'";
				for (int i = 0; i < friendList.length; ++i) {
					String friendFid = friendList[i];
					if(friendFid.equals("") == false) {
						sqlQuery += " or " + "id = " + "'" + friendFid + "'";
					}
				}

				sqlQuery += " ) ";
				System.out.println("getRankingByScoreFromFriend Query = " + sqlQuery);
				ResultSet rs = st.executeQuery(sqlQuery);
				if (rs.next()) {
					return Integer.parseInt(rs.getString(1));
				}


			} catch (SQLException e) {
				// e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}
		return 0;
	}

	public int getGold(String fid) {
		Connection conn = getConnection();
		if (conn != null) {
			try {

				Statement st = conn.createStatement();
				String sqlQuery = "SELECT * FROM score_table WHERE id = " + "'"
						+ fid + "'" + ";";
				ResultSet rs = st.executeQuery(sqlQuery);
				if (rs.next()) {
					return Integer.parseInt(rs.getString("gold"));
				}
			} catch (SQLException e) {
				// e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}
		return 0;
	}

	public int getDice(String fid) {
		Connection conn = getConnection();
		if (conn != null) {
			try {
				Statement st = conn.createStatement();
				String sqlQuery = "SELECT * FROM score_table WHERE id = " + "'"
						+ fid + "'" + ";";
				ResultSet rs = st.executeQuery(sqlQuery);
				if (rs.next()) {
					return Integer.parseInt(rs.getString("dice"));
				}
			} catch (SQLException e) {
				// e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}
		return 0;
	}

	public int getScore(String fid) {
		Connection conn = getConnection();
		if (conn != null) {
			try {

				Statement st = conn.createStatement();
				String sqlQuery = "SELECT * FROM score_table WHERE id = " + "'"
						+ fid + "'" + ";";
				ResultSet rs = st.executeQuery(sqlQuery);
				if (rs.next()) {
					return Integer.parseInt(rs.getString("score"));
				}
			} catch (SQLException e) {
				// e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}
		return 0;
	}

	public int getTutorial(String fid) {
		Connection conn = getConnection();
		if (conn != null) {
			try {

				Statement st = conn.createStatement();
				String sqlQuery = "SELECT * FROM score_table WHERE id = " + "'"
						+ fid + "'" + ";";
				ResultSet rs = st.executeQuery(sqlQuery);
				if (rs.next()) {
					return Integer.parseInt(rs.getString("tutorial"));
				}
			} catch (SQLException e) {
				// e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}

		return 0;
	}
	
	public void setTutorial(String fid,String tutorial) {
		Connection conn = getConnection();
		if (conn != null) {
			try {
				Statement st = conn.createStatement();
				System.out.println("set tutorial");
				String sqlQuery = "UPDATE score_table SET tutorial = " + "'"
						+ tutorial + "'" + "WHERE id = " + "'" + fid + "'" + ";";
				st.executeUpdate(sqlQuery);
			} catch (SQLException e) {
				// e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}
	}

	public boolean IsinUser(String fid) {
		Connection conn = getConnection();

		if (conn != null) {
			try {

				Statement st = conn.createStatement();
				String sqlQuery = "SELECT * FROM score_table WHERE id = " + "'"
						+ fid + "'" + ";";
				ResultSet rs = st.executeQuery(sqlQuery);

				if (rs.next()) {
					// 이미 있는경우
					return true;
				} else {
					return false;
				}

			} catch (SQLException e) {
				// e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}
		return false;
	}

	public int getMessageTime(String sender_fid, String receiver_fid) {
		Connection conn = getConnection();
		if (conn != null) {
			try {

				Statement st = conn.createStatement();
				String sqlQuery = "SELECT * FROM message_table WHERE sender_id = "
						+ "'"
						+ sender_fid
						+ "'"
						+ " AND "
						+ "receiver_id = "
						+ "'"
						+ receiver_fid
						+ "'"
						+ " order by time desc limit 1 " + ";";

				ResultSet rs = st.executeQuery(sqlQuery);
				if (rs.next()) {
					return Integer.parseInt(rs.getString("time"));
				}
			} catch (SQLException e) {
				// e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}

		return 0;
	}

	public int getTime(String fid) {
		Connection conn = getConnection();
		if (conn != null) {
			try {

				Statement st = conn.createStatement();
				String sqlQuery = "SELECT * FROM score_table WHERE id = " + "'"
						+ fid + "'" + ";";
				ResultSet rs = st.executeQuery(sqlQuery);
				if (rs.next()) {
					return Integer.parseInt(rs.getString("time"));
				}
			} catch (SQLException e) {
				// e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}

		return 0;
	}

	public int getCurrentTime() {
		Connection conn = getConnection();
		if (conn != null) {
			try {

				Statement st = conn.createStatement();
				String sqlQuery = "SELECT UNIX_TIMESTAMP();";
				ResultSet rs = st.executeQuery(sqlQuery);
				if (rs.next()) {
					return rs.getInt(1);
				}
			} catch (SQLException e) {
				// e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}

		return 0;
	}

	public ArrayList<messageInfo> getMessageList(String receiver_id) {
		ArrayList<messageInfo> messageList = new ArrayList<messageInfo>();
		Connection conn = getConnection();

		if (conn != null) {
			try {
				Statement st = conn.createStatement();
				String sqlQuery = "SELECT sender_id, name , receiver_id , type from message_table "
						+ " inner join score_table on message_table.sender_id = score_table.id "
						+ " where receiver_id = "
						+ "'"
						+ receiver_id
						+ "'"
						+ ";";
				ResultSet rs = st.executeQuery(sqlQuery);

				while (rs.next()) {
					messageInfo message = new messageInfo();
					message.setReceiveFid(rs.getString("receiver_id"));
					message.setSenderFid(rs.getString("sender_id"));
					message.setType(rs.getString("type"));
					message.setSenderName(rs.getString("name"));
					messageList.add(message);
				}
			} catch (SQLException e) {
				// e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}

		return messageList;
	}

	public void addMessage(messageInfo message, int nTime) {
		Connection conn = getConnection();
		if (conn != null) {
			try {
				Statement st = conn.createStatement();
				String sqlQuery = "INSERT INTO "
						+ " message_table(sender_id,receiver_id,type,time) "
						+ " " + "VALUES(" + "'" + message.getSenderFid() + "'"
						+ "," + "'" + message.getReceiveFid() + "'" + "," + "'"
						+ message.getType() + "'" + "," + "'" + nTime + "'"
						+ ");";
				st.executeUpdate(sqlQuery);

			} catch (SQLException e) {
				// e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}
	}

	public boolean delMessage(messageInfo message) {
		Connection conn = getConnection();
		if (conn != null) {
			try {
				Statement st = conn.createStatement();

				String sqlQuery = "SELECT * FROM message_table WHERE sender_id = "
						+ "'"
						+ message.getSenderFid()
						+ "'"
						+ " AND "
						+ "receiver_id = "
						+ "'"
						+ message.getReceiveFid()
						+ "'"
						+ " AND "
						+ "type = "
						+ "'"
						+ message.getType()
						+ "'" + ";";
				ResultSet rs = st.executeQuery(sqlQuery);

				if (rs.next() == false) {
					// 없는 경우
					return false;
				}

				sqlQuery = "DELETE from message_table " + " WHERE id = " + "'"
						+ rs.getString("id") + "'" + ";";
				st.executeUpdate(sqlQuery);
				return true;

			} catch (SQLException e) {
				// e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}
		return false;
	}

	public void addUser(userInfo user) {
		Connection conn = getConnection();
		if (conn != null) {
			try {
				Statement st = conn.createStatement();
				String sqlQuery = "SELECT * FROM score_table WHERE id = " + "'"
						+ user.getFid() + "'" + ";";
				ResultSet rs = st.executeQuery(sqlQuery);
				System.out.println("add Score data");

				if (rs.next() == false) {
					// 없는 경우
					sqlQuery = "INSERT INTO " + " score_table " + " "
							+ "VALUES(" + "'"
							+ user.getFid()
							+ "'"
							+ ","
							+ "'"
							+ user.getScore()
							+ "'"
							+ ","
							+ "'"
							+ user.getGold()
							+ "'"
							+ ","
							+ "'"
							+ user.getDice()
							+ "'"
							+ ","
							+ "'"
							+ user.getTutorial()
							+ "'"
							+ ","
							+ "'"
							+ user.getName() + "'" + "," + "'" + 0 + "'" + ");";
				}
				st.executeUpdate(sqlQuery);

			} catch (SQLException e) {
				// e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}
	}

	public void setDice(String fid, int nDice, int nTime) {
		Connection conn = getConnection();
		if (conn != null) {
			try {
				Statement st = conn.createStatement();
				System.out.println("set dice");
				String sqlQuery = "UPDATE score_table SET dice = " + "'"
						+ nDice + "'" + "," + " time = " + "'" + nTime + "'"
						+ "WHERE id = " + "'" + fid + "'" + ";";
				st.executeUpdate(sqlQuery);
			} catch (SQLException e) {
				// e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}
	}

	public void setScore(String fid, String scroe) {
		Connection conn = getConnection();
		if (conn != null) {
			try {
				Statement st = conn.createStatement();
				System.out.println("set score");
				String sqlQuery = "UPDATE score_table SET score = " + "'"
						+ scroe + "'" + "WHERE id = " + "'" + fid + "'" + ";";
				st.executeUpdate(sqlQuery);
			} catch (SQLException e) {
				// e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}
	}
	
	public void setGold(String fid, int gold) {
		Connection conn = getConnection();
		if (conn != null) {
			try {
				Statement st = conn.createStatement();
				System.out.println("set gold");
				String sqlQuery = "UPDATE score_table SET gold = " + "'"
						+ gold + "'" + "WHERE id = " + "'" + fid + "'" + ";";
				st.executeUpdate(sqlQuery);
			} catch (SQLException e) {
				// e.printStackTrace();
			} finally { // stop trying
				closeConnection(conn);
			}
		}
	}
}
