# 모션 캡쳐를 활용한 홈 피트니스 <홈피트>
![Image](https://github.com/user-attachments/assets/157175b8-7d63-411f-815d-0cef824d0199) <br/>
모션 트래킹을 활용하여 메타버스 환경에서 다 같이 운동을 할 수 있는 “홈피트” 라는 멀티플레이어 프로그램입니다.
AI 가 웹캠 화면을 통해 파악한 미디어파이프 신체 좌표를 아바타에 적용하여, 사용자의 행동을 아바타가 똑같이 따라하도록 구현하였습니다. API 사용 없이, 모든 기능을 처음부터 끝까지 직접 구현하여 시스템을 완성했습니다.
또한 좌표값의 정확도를 기준으로 사용자가 정확한 동작으로 스쿼트나 팔벌려뛰기를 할 때에만 카운트가 진행되도록 구현했습니다.

## About Project
- **장르**: 홈 피트니스 / 모션 인식
- **작업 기간**: 2024.8.24 - 2024.9.27
- **팀 구성**: 프로그래머 3인, 백엔드 3인, AI 1인
- **담당 파트**: 모션 트래킹 담당
- **키워드**: HTTP 통신, 웹캠 통신, 미디어파이프 좌표 수신 후 아바타에 적용

## 담당 구현 코드
- 모션 인식 : 미디어파이프 가이드라인이 적용된 웹캠 화면을 받아와 인게임 내에 띄워 사용자가 본인의 자세를 확인할 수 있게 함. 어깨와 발 끝이 인식될 때부터 5초 카운트다운을 시작하여, 사용자가 운동을 할 위치를 인지하도록 안내. 
- 아바타 동기화 및 운동 횟수 카운트 : 방장이 운동 종류로 스쿼트/팔벌려뛰기 중 하나를 선택. 선택한 운동에 따라 기준 좌표를 정하고, 그 좌표에 가깝게 움직일 때에만, 즉 사용자의 자세가 정확할 때에만 숫자를 카운트하여 머리 위 UI 에 표시. 플레이어의 움직임을 아바타의 움직임에 동기화.
