# https://docs.opencv.org/4.7.0/d6/d00/tutorial_py_root.html


import cv2
import numpy as math
from PIL import Image
import webbrowser
import time
from scipy import signal

def gkern(gkern_size, sigma): # матрица свёртки
    gkern1d = signal.gaussian(gkern_size, std=sigma).reshape(gkern_size, 1)
    gkern2d = math.outer(gkern1d, gkern1d)
    gkern2d = gkern2d  / math.sum(gkern2d) # нормирование
    return gkern2d

def own_gauss(image, gkern, gkern_size, sigma):
    gray_image_blurred_own = math.zeros(image.shape) # новая картинка (с размерами оригинала), изначально это целиком чёрное изображение (нули)
    for i in range (gkern_size // 2, image.shape[0] - gkern_size//2): # в ширину
        for j in range (gkern_size // 2, image.shape[1] - gkern_size//2): # в высоту
            for a in range(gkern_size):
                for b in range(gkern_size):
                    gray_image_blurred_own[i, j] += gkern[a, b] * image [i - (gkern_size // 2) + a, j - (gkern_size // 2) + b]
    return math.uint8(gray_image_blurred_own)

def main():
    gkern_size = int(input("Введите нечётный размер матрицы больше, чем 1 (например, 3, 5, ...): "))
    sigma = int(input("Введите значение сигмы: "))
    # исходное изображение
    webbrowser.open(r'C:\OpenCV\image.jpg')
    time.sleep(3)
    # конвертация и сохранение изображения в чёрно-белое
    colour_image = cv2.imread("C:\OpenCV\image.jpg")  # считывание цветного изображения
    gray_image = cv2.cvtColor(colour_image, cv2.COLOR_BGR2GRAY)  # в монохромное изображение
    Image.fromarray(gray_image).save("C:\OpenCV\gray_image"+".jpg")
    webbrowser.open(r'C:\OpenCV\gray_image.jpg')
    time.sleep(3)
    # размытие стандартным средством OpenCV
    gray_image_blurred = cv2.GaussianBlur(gray_image, (gkern_size, gkern_size), sigma)
    Image.fromarray(gray_image_blurred).save("C:\OpenCV\gray_image_blurred"+".jpg")
    webbrowser.open(r'C:\OpenCV\gray_image_blurred.jpg')
    time.sleep(3)
    # размытие вручную
    print("Матрица свёртки:\n", gkern(gkern_size, sigma))
    gray_image_blurred_own = own_gauss(gray_image, gkern(gkern_size, sigma), gkern_size, sigma)
    Image.fromarray(gray_image_blurred_own).save("C:\OpenCV\gray_image_blurred_own"+".jpg")
    webbrowser.open(r'C:\OpenCV\gray_image_blurred_own.jpg')

if __name__ == "__main__":
    main()

