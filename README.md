## **개발 환경**

게임 엔진 : Unity

사용 언어 : C#

UI : NGUI  

친구 초대 및 도움주기 : Facebook API  

서버 : Java Servlet / 랭킹 서버

DB : MySQL / 유저 정보 저장  

---

## **구현**

-   랭킹 시스템
-   친구 초대/도움주기 기능
-   페이스북 담벼락 남기기 기능 / 공유 하기 기능
-   다국어 지원 기능
-   결제 기능

---

## 상세 설명

![](https://img1.daumcdn.net/thumb/R1280x0/?scode=mtistory2&fname=https%3A%2F%2Fblog.kakaocdn.net%2Fdn%2Fs4aVd%2Fbtq4dw8JOI8%2FKmK4Ewl8ZeD7ze2r1CxBMK%2Fimg.png)

-   facebook을 통해서 웹환경에서 게임 진행 가능

---

![](https://img1.daumcdn.net/thumb/R1280x0/?scode=mtistory2&fname=https%3A%2F%2Fblog.kakaocdn.net%2Fdn%2FbeKO4R%2Fbtq4bmUeye6%2FRJbEZJK6u54HKdvXK0kSh1%2Fimg.png)

-   랭킹 시스템
    -   플레이 점수를 game server에 저장하고 페이스북 친구리스트를 이용해 랭킹 시스템을 만든다.
    -   서버 개발은 java servlet으로 개발 되었고 DB는 mysql을 이용 하였다.

---

![](https://img1.daumcdn.net/thumb/R1280x0/?scode=mtistory2&fname=https%3A%2F%2Fblog.kakaocdn.net%2Fdn%2F9s3Ca%2Fbtq4biqLQhg%2FhSriaycDArd91Tzqygn5yk%2Fimg.png)
![](https://img1.daumcdn.net/thumb/R1280x0/?scode=mtistory2&fname=https%3A%2F%2Fblog.kakaocdn.net%2Fdn%2Fb3QXku%2Fbtq4bZcXaC0%2FMKNeitiuuSkLlpUQZNfGCk%2Fimg.png)

-   주사위 시스템  
    -   주사위가 있어야 게임을 진행 할수 있고 게임 1판에 주사위 1개씩 차감된다.
    -   주사위는 Facebook 결제를 통해 구매 할수 있고 5개까지는 일정 시간이 지나면 자동으로 충전된다.
    -   페이스북 친구리스트에서 선택한 친구에게 주사위를 보내줄수도 있다. 돕기 기능은 1일 1회 가능  
        

---

![](https://img1.daumcdn.net/thumb/R1280x0/?scode=mtistory2&fname=https%3A%2F%2Fblog.kakaocdn.net%2Fdn%2F6PG6k%2Fbtq4dwt8myi%2FKhFLwDK5vLEgQV6qPlkjAk%2Fimg.png)

-   Game Score 저장
    -   게임이 종료 될때 획득한 점수를 Game Server에 전송해서 저장한다.
    -   획득한 점수가 친구들 사이에서 몇등인지 볼수 있다.

---

![](https://img1.daumcdn.net/thumb/R1280x0/?scode=mtistory2&fname=https%3A%2F%2Fblog.kakaocdn.net%2Fdn%2FGX2aG%2Fbtq4biK7oMa%2FFoA63SAfvMlPffBGhW2xj1%2Fimg.png)
![](https://img1.daumcdn.net/thumb/R1280x0/?scode=mtistory2&fname=https%3A%2F%2Fblog.kakaocdn.net%2Fdn%2F90NPn%2Fbtq4bOCHWC2%2Fos1iPuuqPyGZxtQKqAcX61%2Fimg.png)

-   다국어 지원 기능
    -   KOR / ENG 언어 선택 가능
