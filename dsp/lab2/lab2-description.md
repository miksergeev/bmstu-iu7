Программа получает на вход цветное изображение. Затем
- строится гистограмма яркости исходного изображения
- изображение размывается
- строится гистограмма яркости размытого изображения
- пользователь вводит порог для бинаризации изображения (0...255)
- программа бинаризует изображения с помощью встроенных функций в библиотеке OpenCV (cv2)
- программа бинаризует изображение с помощью вручную написанной функции бинаризации
- реализуется библиотечный метод Оцу (Otsu's method) для поиска оптимального порога яркости
- реализуется вручную написанный метод Оцу (Otsu's method) для поиска оптимального порога яркости
- программа отмечает на гистограмме яркости найденный методом Оцу порог яркости
- реализуется адаптивная бинаризация изображения вручную написанным методом Брэдли