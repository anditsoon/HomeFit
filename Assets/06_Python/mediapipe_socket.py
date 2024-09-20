
# import cv2
# import mediapipe as mp
# import asyncio
# import json
# import socket

# # MediaPipe 초기화
# mp_drawing = mp.solutions.drawing_utils
# mp_pose = mp.solutions.pose

# # UDP 소켓 설정
# server_address = ('localhost', 8764)  # UDP 서버 주소와 포트

# async def process_frame(pose, frame):
#     # BGR 이미지를 RGB로 변환
#     image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
#     image.flags.writeable = False
    
#     # 포즈 추정 수행
#     results = pose.process(image)

#     # 결과 이미지를 BGR로 다시 변환
#     image.flags.writeable = True
#     image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)

#     pose_landmarks = []
#     if results.pose_landmarks:
#         mp_drawing.draw_landmarks(image, results.pose_landmarks, mp_pose.POSE_CONNECTIONS)
#         for landmark in results.pose_landmarks.landmark:
#             pose_landmarks.append({
#                 'x': landmark.x,
#                 'y': landmark.y,
#                 'z': landmark.z
#             })

#     return image, pose_landmarks

# async def send_udp(sock, data):
#     pose_data_json = json.dumps(data).encode('utf-8')
#     sock.sendto(pose_data_json, server_address)

# async def main():
#     print(f"UDP 서버가 {server_address}에서 데이터를 전송합니다.")
#     cap = cv2.VideoCapture(0)
#     sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

#     with mp_pose.Pose(static_image_mode=False, model_complexity=1, enable_segmentation=False, min_detection_confidence=0.5) as pose:
#         while cap.isOpened():
#             ret, frame = cap.read()
#             if not ret:
#                 break

#             image, pose_landmarks = await process_frame(pose, frame)

#             if pose_landmarks:
#                 await send_udp(sock, pose_landmarks)

#             cv2.imshow('Pose Estimation', image)

#             if cv2.waitKey(5) & 0xFF == 27:
#                 break

#     cap.release()
#     cv2.destroyAllWindows()
#     sock.close()

# if __name__ == "__main__":
#     asyncio.run(main())

##### ---------------------------------------------------------------------------------------- #####


import cv2
import mediapipe as mp
import asyncio
import json
import socket
import base64

# MediaPipe 초기화
mp_drawing = mp.solutions.drawing_utils
mp_pose = mp.solutions.pose

# UDP 소켓 설정
server_address = ('localhost', 8764)  # UDP 서버 주소와 포트

async def process_frame(pose, frame):
    # BGR 이미지를 RGB로 변환
    image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    image.flags.writeable = False
    
    # 포즈 추정 수행
    results = pose.process(image)

    # 결과 이미지를 BGR로 다시 변환
    image.flags.writeable = True
    image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)

    pose_landmarks = []
    if results.pose_landmarks:
        mp_drawing.draw_landmarks(image, results.pose_landmarks, mp_pose.POSE_CONNECTIONS)
        for landmark in results.pose_landmarks.landmark:
            pose_landmarks.append({
                'x': landmark.x,
                'y': landmark.y,
                'z': landmark.z
            })

    return image, pose_landmarks

async def send_udp(sock, data, image):
    # 이미지 해상도를 줄여서 크기 축소
    small_image = cv2.resize(image, (320, 240))  # 320x240 해상도로 축소
    
    # 이미지를 JPEG 형식으로 압축 (압축률 50%)
    ret, jpeg_image = cv2.imencode('.jpg', small_image, [int(cv2.IMWRITE_JPEG_QUALITY), 50])
    if ret:
        # 이미지 데이터를 Base64로 인코딩
        image_base64 = base64.b64encode(jpeg_image.tobytes()).decode('utf-8')

        # 좌표 데이터와 이미지를 함께 JSON 형식으로 묶음
        message = {
            'pose_landmarks': data,
            'image': image_base64
        }

        # 데이터를 JSON으로 인코딩하여 전송
        message_json = json.dumps(message).encode('utf-8')
        sock.sendto(message_json, server_address)

async def main():
    print(f"UDP 서버가 {server_address}에서 데이터를 전송합니다.")
    cap = cv2.VideoCapture(0)
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

    with mp_pose.Pose(static_image_mode=False, model_complexity=1, enable_segmentation=False, min_detection_confidence=0.5) as pose:
        while cap.isOpened():
            ret, frame = cap.read()
            if not ret:
                break

            # 프레임 처리 및 포즈 데이터 추출
            image, pose_landmarks = await process_frame(pose, frame)

            # 포즈 데이터 및 이미지 전송
            if pose_landmarks:
                await send_udp(sock, pose_landmarks, image)

            # 결과 프레임을 화면에 표시
            cv2.imshow('Pose Estimation', image)

            # ESC 키로 종료
            if cv2.waitKey(5) & 0xFF == 27:
                break

    cap.release()
    cv2.destroyAllWindows()
    sock.close()

if __name__ == "__main__":
    asyncio.run(main())


## ----------------------------------------------------------------------------------------- ###


# import cv2
# import mediapipe as mp
# import asyncio
# import json
# import socket

# # MediaPipe 초기화
# mp_drawing = mp.solutions.drawing_utils
# mp_pose = mp.solutions.pose

# # UDP 소켓 설정
# server_address = ('localhost', 8764)  # UDP 서버 주소와 포트

# async def process_frame(pose, frame):
#     # BGR 이미지를 RGB로 변환
#     image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
#     image.flags.writeable = False
    
#     # 포즈 추정 수행
#     results = pose.process(image)

#     # 결과 이미지를 BGR로 다시 변환
#     image.flags.writeable = True
#     image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)

#     pose_landmarks = []
#     if results.pose_landmarks:
#         mp_drawing.draw_landmarks(image, results.pose_landmarks, mp_pose.POSE_CONNECTIONS)
#         for landmark in results.pose_landmarks.landmark:
#             pose_landmarks.append({
#                 'x': landmark.x,
#                 'y': landmark.y,
#                 'z': landmark.z
#             })

#     return image, pose_landmarks

# async def send_udp(sock, data):
#     pose_data_json = json.dumps(data).encode('utf-8')
#     sock.sendto(pose_data_json, server_address)

# async def main():
#     print(f"UDP 서버가 {server_address}에서 데이터를 전송합니다.")
#     cap = cv2.VideoCapture(0)
#     sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

#     with mp_pose.Pose(static_image_mode=False, model_complexity=1, enable_segmentation=False, min_detection_confidence=0.5) as pose:
#         while cap.isOpened():
#             ret, frame = cap.read()
#             if not ret:
#                 break

#             image, pose_landmarks = await process_frame(pose, frame)

#             if pose_landmarks:
#                 await send_udp(sock, pose_landmarks)

#             cv2.imshow('Pose Estimation', image)

#             if cv2.waitKey(5) & 0xFF == 27:
#                 break

#     cap.release()
#     cv2.destroyAllWindows()
#     sock.close()

# if __name__ == "__main__":
#     asyncio.run(main())



# import cv2
# import mediapipe as mp
# import asyncio
# import websockets
# import json

# # MediaPipe 초기화
# mp_drawing = mp.solutions.drawing_utils
# mp_pose = mp.solutions.pose

# # 웹캠 캡처 시작
# cap = cv2.VideoCapture(0)

# async def send_pose_data(websocket, path):
#     print("WebSocket 서버가 ws://192.168.0.27:8765 에서 시작되었습니다.")
#     with mp_pose.Pose(static_image_mode=False, model_complexity=1, enable_segmentation=False, min_detection_confidence=0.5) as pose:
#         while cap.isOpened():
#             ret, frame = cap.read()
#             if not ret:
#                 break

#             # BGR 이미지를 RGB로 변환
#             image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
#             image.flags.writeable = False
            
#             # 포즈 추정 수행
#             results = pose.process(image)

#             # 결과 이미지를 BGR로 다시 변환
#             image.flags.writeable = True
#             image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)

#             # 신체 랜드마크 그리기 및 좌표 추출
#             pose_landmarks = []
#             if results.pose_landmarks:
#                 mp_drawing.draw_landmarks(image, results.pose_landmarks, mp_pose.POSE_CONNECTIONS)
#                 for landmark in results.pose_landmarks.landmark:
#                     pose_landmarks.append({
#                         'x': landmark.x,
#                         'y': landmark.y,
#                         'z': landmark.z
#                     })

#                 # 좌표 데이터를 JSON으로 변환하여 전송
#                 pose_data_json = json.dumps(pose_landmarks)
#                 await websocket.send(pose_data_json)

#             # 영상 출력
#             cv2.imshow('Pose Estimation', image)

#             if cv2.waitKey(5) & 0xFF == 27:
#                 break

#     cap.release()
#     cv2.destroyAllWindows()

# # 웹소켓 서버 시작
# start_server = websockets.serve(send_pose_data, "localhost", 8765)


# # 비동기 루프 실행
# asyncio.get_event_loop().run_until_complete(start_server)
# asyncio.get_event_loop().run_forever()
