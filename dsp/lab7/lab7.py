# https://docs.opencv.org/4.x/da/df5/tutorial_py_sift_intro.html
# https://docs.opencv.org/4.x/d7/d60/classcv_1_1SIFT.html

import cv2
import webbrowser
from PIL import Image
import time

def cv2_sift(image_one, image_two, number_of_points=10):
    # to gray
    image_one = cv2.cvtColor(image_one, cv2.COLOR_BGR2GRAY)
    image_two = cv2.cvtColor(image_two, cv2.COLOR_BGR2GRAY)
    # create SIFT object
    sift = cv2.SIFT_create()
    # detect SIFT features in both images
    keypoints_1, descriptors_1 = sift.detectAndCompute(image_one, None)
    keypoints_2, descriptors_2 = sift.detectAndCompute(image_two, None)
    # create feature matcher
    bf = cv2.BFMatcher(cv2.NORM_L1, crossCheck=True)
    # match descriptors of both images
    matches = bf.match(descriptors_1, descriptors_2)
    # sort matches by distance
    matches = sorted(matches, key=lambda x: x.distance)
    # draw first n matches (=10)
    matched_img = cv2.drawMatches(image_one, keypoints_1, image_two, keypoints_2, matches[:number_of_points], image_two, flags=2)
    return matched_img

def main():
    # изображение 1
    image_one = cv2.imread("C:\OpenCV\image1_lab7.jpg")
    webbrowser.open(r'C:\OpenCV\image1_lab7.jpg')
    time.sleep(3)
    # изображение 2
    image_two = cv2.imread("C:\OpenCV\image2_lab7.jpg")
    webbrowser.open(r'C:\OpenCV\image2_lab7.jpg')
    time.sleep(3)
    # запрос количества точек
    number_of_points = int(input("Введите количество характерных точек, которое Вы хотите найти: "))
    # вывод изображения
    matched_image = cv2_sift(image_one, image_two, number_of_points)
    Image.fromarray(matched_image).save("C:\OpenCV\matched_image_lab7" + ".jpg")
    webbrowser.open(r'C:\OpenCV\matched_image_lab7.jpg')
    return 0

if __name__ == '__main__':
    main()


