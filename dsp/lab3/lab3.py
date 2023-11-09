# https://en.wikipedia.org/wiki/Mathematical_morphology
# https://docs.opencv.org/3.4/db/df6/tutorial_erosion_dilatation.html
# https://docs.opencv.org/3.4/d3/dbe/tutorial_opening_closing_hats.html
# ! математическая морфология - только для бинаризованных изображений
# ! значащие пиксели - белые, не значащие - чёрные

import cv2
import numpy as math
import webbrowser
from PIL import Image
import time

# в чёрно-белое изображение
def cv2_togray(image):
    gray_image = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    return gray_image

# бинаризация методом Оцу
def cv2_otsu(image):
    image_otsu = cv2.threshold(image, 0, 255, cv2.THRESH_OTSU)[1]
    return math.uint8(image_otsu)

# функция ввода структурного элемента в виде массива
def generate_kernel():
    print("Генерация структурного элемента")
    element_height = int(input("Введите размер структурного элемента по вертикали (например, 3): "))
    element_width = int(input("Введите размер структурного элемента по горизонтали (например, 3): "))
    element = math.empty((element_height, element_width))
    for i in range (0, element_height):
        for j in range (0, element_width):
            print("Введите элемент массива с индексом [", i, ", ", j, "]: ")
            element[i, j] = bool(input())
    return element

# дилатация (наращивание)
def cv2_dilation(image, element, iterations):
    image = cv2.bitwise_not(image)  # инверсия
    dilated_image = cv2.dilate(image, element, iterations)
    dilated_image = cv2.bitwise_not(dilated_image) # инвертируем обратно
    return dilated_image

# эрозия (сужение)
def cv2_erosion(image, element, iterations):
    image = cv2.bitwise_not(image) # инверсия
    eroded_image = cv2.erode(image, element, iterations)
    eroded_image = cv2.bitwise_not(eroded_image) # инвертируем обратно
    return eroded_image

# замыкание (закрытие)
def closing(image, element, iterations):
    image = cv2_erosion(image, element, iterations) # сначала эрозия
    image = cv2_dilation(image, element, iterations) # потом дилатация
    return image

# размыкание (открытие)
def opening(image, element, iterations):
    image = cv2_dilation(image, element, iterations) # сначала дилатация
    image = cv2_erosion(image, element, iterations) # потом эрозия
    return image

# условная дилатация
def conditional_dilate(image, element1, element2, iterations):
    eroded_image = cv2_erosion(image, element1, iterations) # сначала эрозия с первым элементом (по числу параметра итераций)
    previous_image = eroded_image
    while True:
        previous_image = cv2_dilation(previous_image, element2, iterations) # потом наращивание изображением вторым элементом
        new_image = math.maximum(image, previous_image) # оставляем только то, что было в исходном изображении,
                                                        # максимум используем из-за инверсии в собственных функциях
        if math.array_equal(new_image, previous_image): # если ничего не меняется
            return new_image # можно возвращать изображение
        else:
            previous_image = new_image # иначе продолжаем работать с полученным изображением
            continue

# морфологический скелет
def morphological_skeleton(img, element1):
    img = cv2.bitwise_not(img) # инверсия, т.к. в нашем изображении значащие пиксели - белые
    morphological_skeleton = math.zeros(img.shape, math.uint8) # создаём новое изображение
    while True:
        erode_img = cv2.erode(img, element1)
        dilate_img = cv2.dilate(erode_img, element1)
        subtract = cv2.subtract(img, dilate_img)
        morphological_skeleton = cv2.bitwise_or(morphological_skeleton, subtract)
        img = erode_img.copy()
        if cv2.countNonZero(img) == 0:
            break
    return morphological_skeleton


def main():
    # исходное изображение
    image = cv2.imread("C:\OpenCV\imagelab3.jpg")
    webbrowser.open(r'C:\OpenCV\imagelab3.jpg')
    time.sleep(3)
    # в чёрно-белое
    gray_image = cv2_togray(image)
    Image.fromarray(gray_image).save("C:\OpenCV\image_gray" + ".jpg")
    webbrowser.open(r'C:\OpenCV\image_gray.jpg')
    time.sleep(3)
    # в бинарное
    image_otsu = cv2_otsu(gray_image)
    Image.fromarray(image_otsu).save("C:\OpenCV\image_otsu" + ".jpg")
    webbrowser.open(r'C:\OpenCV\image_otsu.jpg')
    time.sleep(3)
    # генерация структурного элемента
    element1 = generate_kernel()
    # задаём количество итераций
    iterations = int(input("Введите количество итераций: "))
    # дилатация (наращивание)
    dilated_image = cv2_dilation(image_otsu, element1, iterations)
    Image.fromarray(dilated_image).save("C:\OpenCV\image_dilated"+".jpg")
    webbrowser.open(r'C:\OpenCV\image_dilated.jpg')
    time.sleep(3)
    # эрозия (сужение)
    eroded_image = cv2_erosion(image_otsu, element1, iterations)
    Image.fromarray(eroded_image).save("C:\OpenCV\image_eroded"+".jpg")
    webbrowser.open(r'C:\OpenCV\image_eroded.jpg')
    time.sleep(3)
    # замыкание (закрытие)
    closed_image = closing(image_otsu, element1, iterations)
    Image.fromarray(closed_image).save("C:\OpenCV\image_closed"+".jpg")
    webbrowser.open(r'C:\OpenCV\image_closed.jpg')
    time.sleep(3)
    # размыкание (открытие)
    opened_image = opening(image_otsu, element1, iterations)
    Image.fromarray(opened_image).save("C:\OpenCV\image_opened"+".jpg")
    webbrowser.open(r'C:\OpenCV\image_opened.jpg')
    time.sleep(3)
    # условная дилатация
    print("Для операции условной дилатации необходим второй структурный элемент")
    element2 = generate_kernel()
    cond_dil_image = conditional_dilate(image_otsu, element1, element2, iterations)
    Image.fromarray(cond_dil_image).save("C:\OpenCV\image_cond_dil"+".jpg")
    webbrowser.open(r'C:\OpenCV\image_cond_dil.jpg')
    time.sleep(3)
    # морфологический скелет
    skeleton = morphological_skeleton(image_otsu, element1)
    Image.fromarray(skeleton).save("C:\OpenCV\image_skeleton"+".jpg")
    webbrowser.open(r'C:\OpenCV\image_skeleton.jpg')
    time.sleep(3)

if __name__ == "__main__":
    main()
