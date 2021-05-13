package servlet;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;

import org.json.JSONArray;
import org.json.JSONObject;

import bean.productInfo;

public class urlConnection {
	public static productInfo sendToOpenGraphPayment(String id) {
		HttpURLConnection connection = null;
		BufferedReader in = null;
		String inputLine = new String();
		String strResult = new String();

		try {
			// 요청 URL (예를 들어 제 홈페이지로)
			URL url = new URL(
					"https://graph.facebook.com/"
							+ id
							+ "?access_token=453204794790276|NE8ignIcV6aJF5QWPhjHhwenZ6s");
			connection = (HttpURLConnection) url.openConnection();
			// 요청 방식 (GET or POST)
			connection.setRequestMethod("GET");
			// 요청 응답 타임아웃 설정
			connection.setConnectTimeout(3000);
			// 읽기 타임아웃 설정
			connection.setReadTimeout(3000);
			// 컨텐츠의 캐릭터셋이 euc-kr 이라면 (connection.getInputStream(), "euc-kr")
			BufferedReader reader = new BufferedReader(new InputStreamReader(
					connection.getInputStream()));
			StringBuffer buffer = new StringBuffer();
			int read = 0;
			
			char[] cbuff = new char[1024];
			while ((read = reader.read(cbuff)) > 0) {
				buffer.append(cbuff, 0, read);
			}
			reader.close();
			System.out.println(buffer.toString());
			strResult = buffer.toString();
			
			JSONObject object = new JSONObject(strResult);
			if (object == null) {
				System.out.println("object is null");
			}
			
			String status = (String)object.getJSONArray("actions").getJSONObject(0).get("status");
			if(status.equals("completed") == false)
				return null;
			
			productInfo product = new productInfo();
			String name = (String) object.getJSONArray("items").getJSONObject(0).get("product");
			String fid = (String) object.getJSONObject("user").get("id");
			product.setName(name);
			product.setFid(fid);
			System.out.println("name = " + name + "," + "fid = " + fid);
			return product;
			
		} catch (Exception e) {
			e.printStackTrace();
		} finally {
			if (connection != null) {
				connection.disconnect();
			}
		}

		return null;
	}
}
