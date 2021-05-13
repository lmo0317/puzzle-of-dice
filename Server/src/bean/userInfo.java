package bean;

public class userInfo {
	private String fid;
	private String score;
	private String gold;
	private String dice;
	private String tutorial;
	private String name;
	
	public userInfo()
	{
		fid = "-1";
		score = "-1";
		gold = "-1";
		dice = "-1";
		tutorial = "-1";
		name = "-1";
	}
	
	public String getFid() {
		return fid;
	}
	public void setFid(String fid) {
		this.fid = fid;
	}
	public String getScore() {
		return score;
	}
	public void setScore(String score) {
		this.score = score;
	}
	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	public String getGold() {
		return gold;
	}
	public void setGold(String gold) {
		this.gold = gold;
	}
	public String getDice() {
		return dice;
	}
	public void setDice(String dice) {
		this.dice = dice;
	}
	public String getTutorial() {
		return tutorial;
	}
	public void setTutorial(String tutorial) {
		this.tutorial = tutorial;
	}
}
