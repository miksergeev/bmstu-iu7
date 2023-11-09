import cv2
import numpy as math
import webbrowser
from PIL import Image
import time
from matplotlib import pyplot as plt
import warnings
warnings.filterwarnings('ignore')

def own_moravec(image, kernel_size, t): # изображение, размер окна, порог
    image_corners_moravec = image # для выходного изображения
    min_value = math.zeros(image.shape) # нулевая матрица для записи значений изменения интенсивности
    for i in range (1 + kernel_size//2, image.shape[0] - 2 - kernel_size//2): # в высоту
        for j in range(1 + kernel_size//2, image.shape[1] - 2 - kernel_size//2): # в ширину
            shift_right = 0 # изменение интенсивности при сдвиге вправо
            shift_down = 0 # изменение интенсивности при сдвиге вниз
            shift_right_down = 0 # изменение интенсивности при сдвиге по главной диагонали матрицы
            shift_left_down = 0 # изменение интенсивности при сдвиге по побочной диагонали матрицы
            for kernel_counter in range (-kernel_size//2, kernel_size//2 + 1): # в ширину окошка kern
                shift_right += math.square(image[i, j] - image[i + kernel_counter + (kernel_counter + kernel_size//2) // kernel_size, j]) # вправо
                shift_down += math.square(image[i, j] - image[i, j + kernel_counter + (kernel_counter + kernel_size//2) // kernel_size]) # вниз
                shift_right_down += math.square(image[i, j] - image[i + kernel_counter + (kernel_counter + kernel_size//2) // kernel_size,
                                                                    j + kernel_counter + (kernel_counter + kernel_size//2) // kernel_size]) # главная диагональ
                shift_left_down += math.square(image[i, j] - image[i - kernel_counter - (kernel_counter + kernel_size//2) // kernel_size,
                                                                   j + kernel_counter + (kernel_counter + kernel_size//2) // kernel_size]) # побочная диагональ
            min_value[i, j] = min(shift_right, shift_down, shift_right_down, shift_left_down)  # минимальное значение из четырёх

            if min_value[i, j] >= t: # если минимальное значение изменения интенсивности больше порога,
                cv2.circle(image_corners_moravec, (j, i), 5, (0, 0, 0)) # выделяем данную точку
    return image_corners_moravec

    # https://docs.opencv.org/3.4/dc/d0d/tutorial_py_features_harris.html
    # Harris
def cv2_harris(image, block_size=2, aperture_size=3, k=0.04):
    image_float = math.float32(image)
    dst = cv2.cornerHarris(image_float, block_size, aperture_size, k)
    # result is dilated for marking the corners, not important
    dst = cv2.dilate(dst, None)
    # Threshold for an optimal value, it may vary depending on the image.
    for i in range(dst.shape[0]):
        for j in range(dst.shape[1]):
            if int(dst[i, j]) > 0.01 * dst.max():
                cv2.circle(image, (j, i), 1, (0, 0, 0))
    return image

    # FAST
    # https://docs.opencv.org/3.4/df/d0c/tutorial_py_fast.html
def cv2_fast(image, t, non_max_suppression=None):
    image = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    fast = cv2.FastFeatureDetector_create(t, non_max_suppression)
    corners = fast.detect(image, None)
    image_corners_fast = cv2.drawKeypoints(image, corners, None)
    return image_corners_fast

def main():
    # исходное изображение
    image = cv2.imread("C:\OpenCV\image_lab6.jpg")
    gray_image_for_harris = cv2.imread("C:\OpenCV\image_lab6.jpg")
    gray_image_for_harris = cv2.cvtColor(gray_image_for_harris, cv2.COLOR_BGR2GRAY)

    webbrowser.open(r'C:\OpenCV\image_lab6.jpg')
    time.sleep(3)
    # в чёрно-белое
    gray_image = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    gray_image_for_harris = gray_image
    Image.fromarray(gray_image).save("C:\OpenCV\image_gray_lab6" + ".jpg")
    webbrowser.open(r'C:\OpenCV\image_gray_lab6.jpg')
    time.sleep(3)
    # собственная реализация детектора углов Моравеца
    image_corners_moravec = own_moravec(gray_image, 3, 400) # эмпирическим путём для данной картинки подобран порог, равный 400
    Image.fromarray(image_corners_moravec).save("C:\OpenCV\image_corners_moravec_lab6" + ".jpg")
    webbrowser.open(r'C:\OpenCV\image_corners_moravec_lab6.jpg')
    # Harris
    image_corners_harris = cv2_harris(gray_image_for_harris)
    Image.fromarray(image_corners_harris).save("C:\OpenCV\image_corners_harris_lab6" + ".jpg")
    webbrowser.open(r'C:\OpenCV\image_corners_harris_lab6.jpg')
    # FAST
    image_corners_fast = cv2_fast(image, 50, True)
    Image.fromarray(image_corners_fast).save("C:\OpenCV\image_corners_fast_lab6" + ".jpg")
    webbrowser.open(r'C:\OpenCV\image_corners_harris_lab6.jpg')
    return 0

if __name__ == '__main__':
    main()
