package bean;

public class messageInfo {

	private String senderFid;
	private String receiveFid;
	private String senderName;
	private String type; 
	
	public String getSenderFid() {
		return senderFid;
	}
	public void setSenderFid(String senderFid) {
		this.senderFid = senderFid;
	}
	public String getReceiveFid() {
		return receiveFid;
	}
	public void setReceiveFid(String receiveFid) {
		this.receiveFid = receiveFid;
	}
	public String getType() {
		return type;
	}
	public void setType(String type) {
		this.type = type;
	}
	public String getSenderName() {
		return senderName;
	}
	public void setSenderName(String senderName) {
		this.senderName = senderName;
	}
}
