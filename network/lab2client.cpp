//stage 0

#include <iostream>
#include <WinSock2.h>
#include <WS2tcpip.h>
#include <stdio.h>
#include <vector>

#pragma comment(lib, "Ws2_32.lib")

using namespace std;

//stage 1

int main()
{
	WSADATA wsData;

	int erStat = WSAStartup(MAKEWORD(2, 2), &wsData);

	if (erStat != 0) {
		cout << "Error WinSock version initializaion #";
		cout << WSAGetLastError();
		return 1;
	}
	else
		cout << "WinSock initialization is OK" << endl;

	//stage 2

	SOCKET ClientSock = socket(AF_INET, SOCK_STREAM, 0);

	if (ClientSock == INVALID_SOCKET) {
		cout << "Error initialization socket # " << WSAGetLastError() << endl;
		closesocket(ClientSock);
		WSACleanup();
		return 1;
	}
	else
		cout << "Server socket initialization is OK" << endl;

	//stage 3

	in_addr ip_to_num;
	erStat = inet_pton(AF_INET, "127.0.0.1", &ip_to_num);
	if (erStat <= 0) {
		cout << "Error in IP translation to special numeric format" << endl;
		return 1;
	}

	//stage 4

	sockaddr_in servInfo;

	ZeroMemory(&servInfo, sizeof(servInfo));

	servInfo.sin_family = AF_INET;
	servInfo.sin_addr = ip_to_num;	  // Server's IPv4 after inet_pton() function
	servInfo.sin_port = htons(1234);

	erStat = connect(ClientSock, (sockaddr*)&servInfo, sizeof(servInfo));

	if (erStat != 0) {
		cout << "Connection to Server is FAILED. Error # " << WSAGetLastError() << endl;
		closesocket(ClientSock);
		WSACleanup();
		return 1;
	}
	else
		std:: cout << "Connection established SUCCESSFULLY. Ready to send a message to Server" << std:: endl;

	//stage 5 missed

	//stage 6

	vector <char> servBuff(1024), clientBuff(1024);							// Buffers for sending and receiving data
	short packet_size = 0;												// The size of sending / receiving packet in bytes
	

	while (true) {

		cout << "Type your decimal number or type 'exit' to close the connection: ";
		fgets(clientBuff.data(), clientBuff.size(), stdin);

		// Check whether the client would like to close the connection 
		if (clientBuff[0] == 'e' && clientBuff[1] == 'x' && clientBuff[2] == 'i' && clientBuff[3] == 't') 
		{
			packet_size = send(ClientSock, clientBuff.data(), clientBuff.size(), 0);
			std::cout << "Connection is closed" << std::endl;
			shutdown(ClientSock, SD_BOTH);
			closesocket(ClientSock);
			WSACleanup();
			return 0;
		}

		// Check whether the server is available 
		packet_size = send(ClientSock, clientBuff.data(), clientBuff.size(), 0);

		if (packet_size == SOCKET_ERROR) {
			std::cout << "Connection is terminated" << std::endl;
			closesocket(ClientSock);
			WSACleanup();
			return 1;
		}

		// Getting '\n' symbol from server
		packet_size = recv(ClientSock, servBuff.data(), servBuff.size(), 0);
		cout << servBuff.data();
	}
}