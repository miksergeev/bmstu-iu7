StkSeg SEGMENT PARA STACK 'STACK'
	DB 200h DUP (?)
StkSeg ENDS

Input SEGMENT WORD 'DATA'

InputMessageArray DB 13
	DB 'Input the elements of the array (digits only):' , 0Ah ; "Введите элементы матрицы"
	DB '$'

InputMessageArrayWarning DB 13
	DB 'Wrong type. Only digits are allowed.' , 0Ah ; "Неверно. Разрешён ввод только цифр"
	DB '$'
	
InputMessageRC DB 13
	DB 'Input the number of rows/columns (from 1 up to 9):' , 0Ah ; "Введите количество строк/столбцов"
	DB '$'

InputMessageRCWarning DB 13
	DB 'Input only numbers from 1 to 9!' , 0Ah	; "Введите количество строк/столбцов"
	DB '$'

StringArray DB 101, 101 DUP (?)

Input ENDS

Code SEGMENT WORD 'CODE'
	ASSUME CS:Code, DS:Input
	
DispMsg:	

	
InputArray: ; ввод элементов массива

	MOV AX, Input
	MOV DS, AX 
	MOV DX, OFFSET InputMessageArray ; выводим приглашение к вводу
	MOV AH, 9
	INT 21h 
	
	MOV DX, OFFSET StringArray
	MOV AH, 0Ah; вводим строку
	INT 21h
	
InputArrayCheck: ; проверка элементов (допустимы только цифры)
MOV SI, 00B0h ; B0h - сегмент начала введённой строки (количество элементов)
MOV CL, byte ptr [SI] ; переносим количество внесённых элементов в CX - будем использовать как счётчик цикла проверки введённых значений
                 ; (наша матрица должна состоять лишь из цифр)
XOR CH, CH       ; обнуляем CH

InputArrayCheckLabel1: ; начинаем проверку введённых элементов
ADD SI, 1
CMP byte ptr [SI], 30h
JB MessageArrayWarning
CMP byte ptr [SI], 39h
JA MessageArrayWarning
loop InputArrayCheckLabel1

JMP InputArrayCheckLabel1OK ;					если проверка пройдена - переходим к следующему этапу

MessageArrayWarning: ;  						если хоть один элемент - не цифра,
MOV DX, OFFSET InputMessageArrayWarning ;		выводим соответствующее сообщение
MOV AH, 9 
INT 21h
JMP InputArray ;						  		и отправляем вводить заново

InputArrayCheckLabel1OK: ;						если проверка пройдена - присвоим невведённым элементам значение "нуль",
MOV CX, 0064h;									количество таких элементов равно 64h(=100d) ...
SUB CL, [DS:00B0h] ;																		... минус количество ввёденных
MOV AX, 00B0h
ADD AX, [DS:00B0h]
XOR AH, AH
MOV SI, AX ; для организации цикла сейчас мы указали сегмент последней введённой цифры

Null: ; с помощью цикла обнуляем невведённые значения
ADD SI, 1
MOV byte ptr [SI], 30h
loop Null

RowColumnRequest: ; спросим про размер матрицы
MOV DX, OFFSET InputMessageRC
MOV AH, 9
INT 21h

MOV AH, 01h ; максимальный размер матрицы 9x9 позволяет реализовать ввод размера матрицы с помощью одного символа
INT 21h

CMP AL, 31h ; сравним введенное число с единицей.
JB RCWarning ; если меньше - выводим предупреждение
CMP AL, 39h ; и с девяткой
JA RCWarning ; если больше - тоже выводим предупреждение

SUB AL, 30h ; сохраним в AL собственно размер матрицы
MOV DH, AL ; и перенесём в DH
XOR AL, AL
JMP OutputStep1Initial ; а если между 1 и 9 - переходим к выводу

RCWarning: ; предупреждение
MOV DX, OFFSET InputMessageRCWarning
MOV AH, 9
INT 21h
MOV DL, 0Ah
MOV AH, 2
INT 21h
JMP RowColumnRequest ; и повторный вопрос про размер матрицы

OutputStep1Initial: ; к этому этапу у нас есть размер матрицы в DH, а невведённые элементы равны нулю. Можно выводить исходную матрицу

XOR AX, AX
XOR BX, BX
XOR CX, CX ; на всякий случай

MOV SI, 00B0h ;

MOV CL, DH ; будем использовать размер матрицы в цикле

MOV AH, 02
MOV DL, 0Ah
INT 21h ; отступ

MOV AH, 08
INT 21h ; вывод по нажатию клавиши

Call OutputProg
JMP Step2

OutputProg proc

OutputArray1:

MOV AH, 02
MOV DL, 0Ah
INT 21h ; отступим для вывода матрицы (и каждой новой строки)

PUSH CX ; перед следующим циклом сохраним то значение CL, которое было, в BL
MOV CL, DH
OutputArray1String:

ADD SI, 1
MOV AH, 02
MOV DL, byte ptr [SI] ; выводим элемент
INT 21h

MOV AH, 02
MOV DL, " "
INT 21h
loop OutputArray1String

POP CX

loop OutputArray1

RET

OutputProg endp

Step2:


OutputStep2Transposed: ; в DH - по прежнему размер матрицы

XOR AX, AX
XOR BX, BX
XOR CX, CX ; на всякий случай

MOV CL, DH

MOV AH, 02
MOV DL, 0Ah
INT 21h ; отступ

MOV AH, 08
INT 21h ; вывод по нажатию клавиши


OutputArray2:

MOV AH, 02
MOV DL, 0Ah
INT 21h ; отступим для вывода матрицы (и каждой новой строки)

MOV SI, 00B0h ;
SUB SI, CX
ADD SI, 1

MOV BL, CL ; сохраним текущее значение счётчика цикла в BL
MOV CL, DH

OutputArray2String:

MOV BH, CL ; перенесём пока CL в BH
MOV CL, DH
ADD SI, CX ; прибавляем размер матрицы
MOV CL, BH ; возвращаем счётчик
XOR BH, BH

MOV AH, 02
MOV DL, byte ptr [SI] ; выводим элемент
INT 21h

MOV AH, 02
MOV DL, " "
INT 21h
loop OutputArray2String

MOV CL, BL

loop OutputArray2

OutputArray3Calculation: ; здесь рассчитаем, в каком столбце наибольшая сумма элементов

MOV [DS:00B0h], DH ; сохраним размер матрицы в нулевом элементе введённой строки. 
				   ; Тем самым освободим регистры, использование которых будет удобно нам далее и в то же время будем уверены, что
				   ; ячейка 00B0h точно не перезапишется при выполнении программы.

MOV CL, byte ptr [DS:00B0h] ; используем его как счётчик
MOV SI, 00B0h
XOR DX, DX ; в DH будем складывать элементы каждого столбца для последующего сравнения
XOR BX, BX ; в BH будет максимальное число, в BL - номер такого столбца

OutputArray3Calc:

MOV DL, CL ; перед следующим циклом сохраним то значение CL, которое было, в DL
MOV CL, [DS:00B0h]

OutputArray3StringCalc:

ADD SI, 1
ADD DH, byte ptr [SI] ; выводим элемент
SUB DH, 30h

loop OutputArray3StringCalc

CMP DH, BH

JB Skip
MOV BH, DH
MOV BL, byte ptr [DS:00B0h]
SUB BL, DL
ADD BL, 1

Skip:
XOR DH, DH
MOV CL, DL

loop OutputArray3Calc


; MOV DL, BL 						
; ADD DL, 30h			; Теперь в BL у нас - номер столбца с наибольшой суммой элементов.	
; MOV AH, 02			; Перенесём этот номер в DH и выведем матрицу без этого столбца.
; INT 21h				; Этот кусок кода оставлен для вывода номера столбца с наибольшей суммой элементов

MOV DH, BL



OutputStep3Transposed: 

XOR AX, AX
XOR CX, CX ; на всякий случай



MOV AH, 02
MOV DL, 0Ah
INT 21h ; отступ

MOV AH, 08
INT 21h ; вывод по нажатию клавиши

MOV CL, byte ptr [DS:00B0h]

OutputArray3:

MOV AH, 02
MOV DL, 0Ah
INT 21h ; отступим для вывода матрицы (и каждой новой строки)

MOV SI, 00B0h ;
SUB SI, CX
ADD SI, 1

MOV BL, CL ; сохраним текущее значение счётчика цикла в BL
MOV CL, byte ptr [DS:00B0h]

OutputArray3String:

MOV BH, CL ; перенесём пока CL в BH
MOV CL, byte ptr [DS:00B0h]
ADD SI, CX ; прибавляем размер матрицы
MOV CL, BH ; возвращаем счётчик
XOR BH, BH

MOV AH, byte ptr [DS:00B0h]
SUB AH, CL
ADD AH, 1

CMP AH, DH
JE Skip2

MOV AH, 02
MOV DL, byte ptr [SI] ; выводим элемент
INT 21h

MOV AH, 02
MOV DL, " "
INT 21h

Skip2:

loop OutputArray3String

MOV CL, BL

loop OutputArray3



MOV CL, byte ptr [DS:00B0h] ; по-прежнему используем размер матрицы как счётчик цикла
XOR AX, AX
XOR BX, BX
XOR DX, DX

OutputArray4Calculation: ; здесь рассчитаем, в какой строке больше всего нечётных элементов, в DH будем складывать количество нечётных,
						 ; в BH - максимальное число нечётных, в AH - номер такой строки



MOV SI, 00B0h ;
SUB SI, CX
ADD SI, 1

MOV BL, CL ; сохраним текущее значение счётчика цикла в BL
MOV CL, byte ptr [DS:00B0h]

OutputArray4String:

MOV DL, CL ; перенесём пока CL в DL
MOV CL, byte ptr [DS:00B0h]
ADD SI, CX ; прибавляем размер матрицы
MOV CL, DL ; возвращаем счётчик
XOR DL, DL


MOV DL, byte ptr [SI] ; помещаем элемент в DL
TEST DL, 1

JNZ PlusOdd
JMP Skip3

PlusOdd:
ADD DH, 1


Skip3:

loop OutputArray4String

CMP DH, BH

JB Skip4

MOV BH, DH ; сохраняем максимальное количество нечётных в BH

MOV AH, byte ptr [DS:00B0h]
SUB AH, BL 
ADD AH, 1 

MOV AL, AH ; в AL помещаем номер строки с наибольшим количеством нечётных цифр

XOR DX, DX
Skip4:

MOV CL, BL ; возвращаем из BL счётчик


loop OutputArray4Calculation


MOV DL, AL 			; в AL теперь находится номер строки с наибольшим числом нечётных цифр
MOV DH, AL ; перенесём номер такой строки в DH

OutputStep4Output: 

XOR AX, AX
XOR BX, BX
XOR CX, CX ; на всякий случай

MOV CL, [DS:00B0h]

MOV AH, 02
MOV DL, 0Ah
INT 21h ; отступ

MOV AH, 02
MOV DL, 0Ah
INT 21h ; отступ

MOV AH, 08
INT 21h ; вывод по нажатию клавиши

OutputArray4Output:

MOV SI, 00B0h ;
SUB SI, CX
ADD SI, 1

MOV BL, CL ; сохраним текущее значение счётчика цикла в BL
MOV CL, [DS:00B0h]

OutputArray4StringOutput:

MOV BH, CL ; перенесём пока CL в BH
MOV CL, [DS:00B0h]
ADD SI, CX ; прибавляем размер матрицы
MOV CL, BH ; возвращаем счётчик
XOR BH, BH

MOV AH, 02
MOV DL, byte ptr [SI] ; выводим элемент
INT 21h

MOV AH, 02
MOV DL, " "
INT 21h
loop OutputArray4StringOutput

MOV CL, BL

MOV AH, 02
MOV DL, 0Ah
INT 21h ; отступим для вывода каждой новой строки

loop OutputArray4Output

MOV AH, DH

XOR CX, CX
MOV CL, [DS:00B0h]
MOV BX, CX
MOV AL, 00B0h

SUB AL, CL
ADD AL, AH
XOR AH, AH
MOV SI, AX

OutputArray4OneMoreStringOutput:
MOV BH, CL ; перенесём пока CL в BH
MOV CL, [DS:00B0h]
ADD SI, CX ; прибавляем размер матрицы
MOV CL, BH ; возвращаем счётчик
XOR BH, BH

TEST byte ptr [SI], 1

JZ Ev

Odd:
MOV AH, 02
MOV DL, 30h ; выводим нуль
INT 21h


JMP Skip5

Ev:
MOV AH, 02
MOV DL, byte ptr [SI] ; выводим элемент
INT 21h


Skip5:
MOV AH, 02
MOV DL, " "
INT 21h


loop OutputArray4OneMoreStringOutput

TheEnd:

MOV AH, 4Ch ; завершаем процесс
INT 21h

Code ENDS
END DispMsg