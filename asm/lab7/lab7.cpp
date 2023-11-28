#include <iostream>
#include <string>


int main()
{
	int i(0); // i - указатель строки

    std::string s("Hello World!");
	std::cout << s; // вывод строки

	std::cout << "\nThe string pointer is :\t"; // "указатель равен ... "

	void * firstItem = &s[0]; // передаём подпрограмме указатель на строку
	__asm {
		mov eax, firstItem
		sub eax, 4
		mov i, eax // в ассемблерной вставке передаём его переменной i
	}
	std::cout << std::hex << i;
	std::cout << "\n";
	std::cout << &s;
	getchar();
}
