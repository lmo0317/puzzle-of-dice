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

## **소개 영상**

---

## 상세 설명

[##_Image|kage@s4aVd/btq4dw8JOI8/KmK4Ewl8ZeD7ze2r1CxBMK/img.png|alignCenter|data-filename="스크린샷 2014-06-30 05.12.25.png" data-origin-width="1600" data-origin-height="868" data-ke-mobilestyle="widthContent"|||_##]

-   facebook을 통해서 웹환경에서 게임 진행 가능

---

[##_Image|kage@beKO4R/btq4bmUeye6/RJbEZJK6u54HKdvXK0kSh1/img.png|alignCenter|data-filename="스크린샷 2014-06-30 05.12.16.png" data-origin-width="1600" data-origin-height="868" data-ke-mobilestyle="widthContent"|||_##]

-   랭킹 시스템
    -   플레이 점수를 game server에 저장하고 페이스북 친구리스트를 이용해 랭킹 시스템을 만든다.
    -   서버 개발은 java servlet으로 개발 되었고 DB는 mysql을 이용 하였다.

---

[##_Image|kage@9s3Ca/btq4biqLQhg/hSriaycDArd91Tzqygn5yk/img.png|alignCenter|data-filename="스크린샷 2014-06-30 05.12.53.png" data-origin-width="1600" data-origin-height="868" data-ke-mobilestyle="widthContent"|||_##][##_Image|kage@b3QXku/btq4bZcXaC0/MKNeitiuuSkLlpUQZNfGCk/img.png|widthContent|data-origin-width="595" data-origin-height="516" data-ke-mobilestyle="widthContent"|||_##]

-   주사위 시스템  
    -   주사위가 있어야 게임을 진행 할수 있고 게임 1판에 주사위 1개씩 차감된다.
    -   주사위는 Facebook 결제를 통해 구매 할수 있고 5개까지는 일정 시간이 지나면 자동으로 충전된다.
    -   페이스북 친구리스트에서 선택한 친구에게 주사위를 보내줄수도 있다. 돕기 기능은 1일 1회 가능  
        

---

[##_Image|kage@6PG6k/btq4dwt8myi/KhFLwDK5vLEgQV6qPlkjAk/img.png|alignCenter|data-filename="스크린샷 2014-06-30 05.31.21.png" data-origin-width="1600" data-origin-height="868" data-ke-mobilestyle="widthContent"|||_##]

-   Game Score 저장
    -   게임이 종료 될때 획득한 점수를 Game Server에 전송해서 저장한다.
    -   획득한 점수가 친구들 사이에서 몇등인지 볼수 있다.

---

[##_Image|kage@GX2aG/btq4biK7oMa/FoA63SAfvMlPffBGhW2xj1/img.png|widthContent|data-origin-width="600" data-origin-height="513" data-ke-mobilestyle="widthContent"|||_##][##_Image|kage@90NPn/btq4bOCHWC2/os1iPuuqPyGZxtQKqAcX61/img.png|widthContent|data-origin-width="600" data-origin-height="516" data-ke-mobilestyle="widthContent"|||_##]

-   다국어 지원 기능
    -   KOR / ENG 언어 선택 가능
