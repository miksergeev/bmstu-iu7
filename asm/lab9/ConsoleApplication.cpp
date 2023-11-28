// ConsoleApplication23.cpp: ���������� ����� ����� ��� ����������� ����������.
//


#pragma warning (disable : 4996)

int _tmain(int argc, _TCHAR* argv[])
{
	const UINT CodePage = 1251 ;
	setlocale ( LC_ALL , "rus" ) ;
	SetConsoleCP ( CodePage ) ;
	SetConsoleOutputCP ( CodePage ) ;

	double a (0.0) , b (0.0) , c (0.0) ;
	double p (0.0) ;
	double s (0.0) ;
	printf ( "\t�������  ������� ������������ : " ) ;
	scanf ( "%lf%lf%lf" , &a, &b, &c ) ;

	if ( ( a <= 0.0 ) || (b <= 0.0 ) || ( c <= 0.0 ) )
	{
		printf ( "\t������� ������������� ��� ������� ����� ������ ������������\n" ) ;
		getch () ;
		return 1;
	} 
	if ( (a+b<=c) || (a+c<=b) || (b+c<=a) )
	{
		printf ( "\t����������� �� ����������" ) ;
		getch () ;
		return 2;
	}

	p = ((a+b+c)/2.0);
	s = sqrt(p*(p-a)*(p-b)*(p-c));
	printf ( "\t������� ������������ ����� : %.2lf\n" , s );

	getch () ;
	return 0;
}

