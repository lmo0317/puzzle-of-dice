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
			// ��û URL (���� ��� �� Ȩ��������)
			URL url = new URL(
					"https://graph.facebook.com/"
							+ id
							+ "?access_token=453204794790276|NE8ignIcV6aJF5QWPhjHhwenZ6s");
			connection = (HttpURLConnection) url.openConnection();
			// ��û ��� (GET or POST)
			connection.setRequestMethod("GET");
			// ��û ���� Ÿ�Ӿƿ� ����
			connection.setConnectTimeout(3000);
			// �б� Ÿ�Ӿƿ� ����
			connection.setReadTimeout(3000);
			// �������� ĳ���ͼ��� euc-kr �̶�� (connection.getInputStream(), "euc-kr")
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
