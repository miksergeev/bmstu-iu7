import cv2
import numpy as math # https://numpy.org/doc/stable/index.html
from PIL import Image
import webbrowser
import time
from matplotlib import pyplot as plt

# функция для построения гистограммы яркости
def diagram(image):
    bins = [0 for i in range(256)] # массив нулей [256]
    for i in range (0, image.shape[0]): # в ширину
        for j in range (0, image.shape[1]): # в высоту
                bins[image[i, j]] +=1
    return bins

# пороговая бинаризация библиотечная
def opencv2_threshold(image, threshold):
    image_threshold = cv2.threshold(image, threshold, 255, cv2.THRESH_BINARY)[1]
    return math.uint8(image_threshold) # обработанное изображение

# пороговая бинаризация вручную
def own_threshold(image, threshold):
    image_threshold = math.zeros(image.shape)
    for i in range (0, image.shape[0]): # в ширину
        for j in range (0, image.shape[1]): # в высоту
            if (image[i, j] > threshold):
                image_threshold[i, j] = 255
            else:
                image_threshold[i, j] = 0
    return math.uint8(image_threshold) # обработанное изображение

# метод Оцу библиотечный
def cv2_otsu(image):
    threshold, image_threshold = cv2.threshold(image, 0, 255, cv2.THRESH_OTSU)
    return math.uint8(threshold), math.uint8(image_threshold) # найденный порог, обработанное изображение

# метод Оцу вручную # https://ru.wikipedia.org/wiki/%D0%9C%D0%B5%D1%82%D0%BE%D0%B4_%D0%9E%D1%86%D1%83
def own_otsu(image):
    bins = diagram(image) # гистограмма высот
    sm = sum(bins) # количество точек
    smI2 = 0 # суммарная интенсивность точек во втором классе
    for i in range (0, 256):
        smI2 += bins[i] * i # (т.е. всех)
    T = 0 # искомый порог
    max_sigma = 0.0
    count1 = 0 # точек в первом классе
    count2 = sm # точек во втором классе
    smI1 = 0 # суммарная интенсивность точек в первом классе
    for thresh in range (1, 254):
        count1 += bins[thresh]
        if count1 == 0:
            continue
        if count2 - count1 == 0:
            continue
        smI1 += thresh * bins[thresh]
        w1 = count1 / float(sm)
        w2 = 1.0 - w1
        mu1 = smI1 / float(count1)
        mu2 = (smI2 - smI1) / float(count2 - count1)
        d = float(mu1 - mu2)
        sigma = float(w1 * w2 * d * d)
        if (sigma > max_sigma):
            max_sigma = sigma
            T = thresh
    return T

# метод Брэдли # https://habr.com/ru/articles/278435/
def own_bradley(image, T = 15.0, Sdiv = 0.125):
    width = image.shape[0] # ширина
    height = image.shape[1] # высота
    S = width * Sdiv # размер окна
    s2 = S / 2
    integral_image = math.zeros_like(image, math.uint32) # создаём массив для интегрального изображения
    for i in range(0, width): # цикл по столбцам
        for j in range(0, height): # цикл по строкам
            integral_image[i, j] = math.uint64(image[0:i, 0:j].sum()) # суммы яркости на местах "пикселей" в интегральном изображении
    output_image = math.zeros_like(image) # массив нулей для вывода обработанного изображения
    for x in range(width): # цикл по столбцам x
        for y in range(height): # цикл по строкам y
            x0 = int(max(x - s2, 0))
            x1 = int(min(x + s2, height - 1))
            y0 = int(max(y - s2, 0))
            y1 = int(min(y + s2, width - 1))
            count = (x1 - x0) * (y1 - y0)
            Sa = math.uint64(integral_image[x1, y1] - integral_image[x0, y1] - integral_image[x1, y0] + integral_image[x0, y0])
            if image[x, y] * count < Sa * (100.0 - T) / 100.0:
                output_image[x,y] = 0
            else:
                output_image[x,y] = 255
    return output_image

def main():
    # исходное изображение
    webbrowser.open(r'C:\OpenCV\imagelab2.jpg')
    time.sleep(3)
    # конвертация и сохранение изображения в чёрно-белое
    colour_image = cv2.imread("C:\OpenCV\imagelab2.jpg")  # считывание цветного изображения
    gray_image = cv2.cvtColor(colour_image, cv2.COLOR_BGR2GRAY)  # в монохромное изображение
    Image.fromarray(gray_image).save("C:\OpenCV\gray_image"+".jpg")
    webbrowser.open(r'C:\OpenCV\gray_image.jpg')
    time.sleep(3)
    '''
    # сглаживание
    gray_image_blurred = cv2.GaussianBlur(gray_image, (9, 9), 1)
    Image.fromarray(gray_image_blurred).save("C:\OpenCV\gray_image_blurred"+".jpg")
    webbrowser.open(r'C:\OpenCV\gray_image_blurred.jpg')
    time.sleep(3)
    '''
    gray_image_blurred = gray_image
    # гистограмма яркости не сглаженного изображения
    plt.plot(range(256), diagram(gray_image))
    plt.title("Not blurred")
    plt.savefig('C:\OpenCV\diagram_not_blurred.png')
    plt.clf()
    webbrowser.open(r'C:\OpenCV\diagram_not_blurred.png')
    time.sleep(3)
    # гистограмма яркости сглаженного изображения
    plt.plot(range(256), diagram(gray_image_blurred))
    plt.title("Blurred")
    plt.savefig('C:\OpenCV\diagram_blurred.png')
    plt.clf()
    webbrowser.open(r'C:\OpenCV\diagram_blurred.png')
    time.sleep(3)
    # простая пороговая бинаризация
    threshold = int(input("Введите порог: "))
    # простая пороговая бинаризация, библиотечный метод
    image_threshold_cv2 = opencv2_threshold(gray_image_blurred, threshold)
    Image.fromarray(image_threshold_cv2).save("C:\OpenCV\image_threshold_cv2"+".jpg")
    webbrowser.open(r'C:\OpenCV\image_threshold_cv2.jpg')
    time.sleep(3)
    # простая пороговая бинаризация, вручную
    image_threshold_own = own_threshold(gray_image_blurred, threshold)
    Image.fromarray(image_threshold_own).save("C:\OpenCV\image_threshold_own"+".jpg")
    webbrowser.open(r'C:\OpenCV\image_threshold_own.jpg')
    time.sleep(3)
    # бинаризация методом Оцу
    # бинаризация библиотечным методом Оцу
    image_otsu_cv2 = cv2_otsu(gray_image_blurred)[1]
    Image.fromarray(image_otsu_cv2).save("C:\OpenCV\image_otsu_cv2"+".jpg")
    webbrowser.open(r'C:\OpenCV\image_otsu_cv2.jpg')
    print("Порог, найденный библиотечным методом Оцу: ", cv2_otsu(gray_image_blurred)[0])
    time.sleep(3)
    # бинаризация методом Оцу вручную
    print("Порог, найденный самостоятельно реализованным методом Оцу: ", own_otsu(gray_image_blurred))
    # гистограмма с отмеченным порогом
    mask = own_otsu(gray_image_blurred)
    plt.plot(range(256), diagram(gray_image_blurred))
    plt.scatter(range(256)[mask], diagram(gray_image_blurred)[mask], color='red', s=60, marker='o')
    plt.title("Blurred with thresh")
    plt.savefig('C:\OpenCV\diagram_blurred_thresh.png')
    plt.clf()
    webbrowser.open(r'C:\OpenCV\diagram_blurred_thresh.png')
    # адаптивная бинаризация методом Брэдли
    T = float(input("Введите значение параметра T для метода Брэдли (по умолчанию T = 15): "))
    Sdiv = float(input("Введите значение размера ширины окна для метода Брэдли (по умолчанию ширина такого окна равна 0.125 от размера изображения): "))
    image_otsu_bradley = own_bradley(gray_image_blurred, T, Sdiv)
    Image.fromarray(image_otsu_bradley).save("C:\OpenCV\image_bradley"+".jpg")
    webbrowser.open(r'C:\OpenCV\image_bradley.jpg')

if __name__ == "__main__":
    main()
