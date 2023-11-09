//stage 0

#include <iostream>
#include <WinSock2.h>
#include <WS2tcpip.h>
#include <stdio.h>
#include <vector>
#include <string>

#pragma comment(lib, "Ws2_32.lib")

using namespace std;

// functions for binary and ternary

string number(int decNumber, int base)
{
	string num = "";
	while (decNumber > 0)
	{
		binNum.insert(num.begin(), (decNumber % base) + '0');
		decNumber /= base;
	}
	return num;
}

//stage 1

int main()
{
	WSADATA wsData;

	int erStat = WSAStartup(MAKEWORD(2, 2), &wsData);

	if (erStat != 0) {
		std::cout << "Error WinSock version initializaion #";
		std::cout << WSAGetLastError();
		return 1;
	}
	else
		std::cout << "WinSock initialization is OK" << std::endl;

	//stage 2

	SOCKET ServSock = socket(AF_INET, SOCK_STREAM, 0);

	if (ServSock == INVALID_SOCKET) {
		std::cout << "Error initialization socket # " << WSAGetLastError() << std::endl;
		closesocket(ServSock);
		WSACleanup();
		return 1;
	}
	else
		std::cout << "Server socket initialization is OK" << std::endl;

	//stage 3

	in_addr ip_to_num;
	erStat = inet_pton(AF_INET, "127.0.0.1", &ip_to_num);
	if (erStat <= 0) {
		std::cout << "Error in IP translation to special numeric format" << std::endl;
		return 1;
	}

	sockaddr_in servInfo;
	ZeroMemory(&servInfo, sizeof(servInfo));

	servInfo.sin_family = AF_INET;
	servInfo.sin_addr = ip_to_num;
	servInfo.sin_port = htons(1234);

	erStat = bind(ServSock, (sockaddr*)&servInfo, sizeof(servInfo));
	if (erStat != 0) {
		std::cout << "Error Socket binding to server info. Error # " << WSAGetLastError() << std::endl;
		closesocket(ServSock);
		WSACleanup();
		return 1;
	}
	else
		std::cout << "Binding socket to Server info is OK" << std::endl;

	//stage 4

	erStat = listen(ServSock, SOMAXCONN);

	if (erStat != 0) {
		std:: cout << "Can't start to listen to. Error # " << WSAGetLastError() << std::endl;
		closesocket(ServSock);
		WSACleanup();
		return 1;
	}
	else {
		std::cout << "Listening..." << std::endl;
	}

	//stage 5

	sockaddr_in clientInfo;

	ZeroMemory(&clientInfo, sizeof(clientInfo));

	int clientInfo_size = sizeof(clientInfo);

	SOCKET ClientConn = accept(ServSock, (sockaddr*)&clientInfo, &clientInfo_size);

	if (ClientConn == INVALID_SOCKET) {
		std::cout << "Client detected, but can't connect to a client. Error # " << WSAGetLastError() << std::endl;
		closesocket(ServSock);
		closesocket(ClientConn);
		WSACleanup();
		return 1;
	}
	else
		std::cout << "Connection to a client established successfully" << std::endl;

	//stage 6

	vector <char> servBuff(1024), clientBuff(1024);
	short packet_size = 0;


	int decNumber;

	while (true)
	{

		packet_size = recv(ClientConn, servBuff.data(), servBuff.size(), 0);

		if (packet_size == SOCKET_ERROR) {
			cout << "Connection is terminated" << endl;
			servBuff.clear();
			clientBuff.clear();
			closesocket(ServSock);
			closesocket(ClientConn);
			WSACleanup();
			return 0;
		}

		if ((servBuff[0] == 'e') && (servBuff[1] == 'x') && (servBuff[2] == 'i') && (servBuff[3] == 't'))
		{
			std::cout << "Client has closed the connection" << std::endl;
			servBuff.clear();
			clientBuff.clear();
			closesocket(ServSock);
			closesocket(ClientConn);
			WSACleanup();
			return 0;
		}

		else

		{
			sscanf_s(servBuff.data(), "%d", &decNumber);
			std::cout << "Decimal number: " << std::dec << decNumber << std::endl;
			std::cout << "Binary number: " << number(decNumber, 2) << std::endl;
			std::cout << "Hex number: " << std::hex << decNumber << std::endl;
			std::cout << "Oct number: " << std::oct << decNumber << std::endl;
			std::cout << "Ternary number: " << number(decNumber, 3) << std::endl;
			std::cout << std::endl;

			packet_size = send(ClientConn, "\n", servBuff.size(), 0);
		}
	}
}