#Division

def main():
    print('Division check')
    while True:
        try:
            print('Input Dividend(integer):')
            Dividend = int(input())
            print('Input Divisor(integer):')
            Divisor = int(input())
            break
        except:
            print("That's not a valid option!")
    print('__________________________________________________________________________________')
    print('Dividend = ', Dividend)
    print('Divisor = ', Divisor)

    Result = divide(Dividend, Divisor)

    print('__________________________________________________________________________________')
    print()
    print('Result: ', Result)
    print('Expected result: ', int(Dividend/Divisor))
    print('Remainder: ', int(Dividend)%int(Divisor))
    if(Result == Dividend//Divisor):
        print('\033[92m Whoa, it worked!! \033[0m')
    else:
        print('\033[91m Whoa, it not worked!! \033[0m')
    print()


def divide(Dividend, Divisor):
    binaryDividend = shiftToNBit(bin(Dividend).lstrip('0b'), n=32)
    print('Dividend: ' + binaryDividend)
    
    binaryDivisor = bin(Divisor).lstrip('0b')
    while len(binaryDivisor) < 32:
        binaryDivisor += '0'
    print('Divisor:  ' + binaryDivisor)
    
    binaryRegister = binaryDividend + shiftToNBit('', n=32)
    print('Initial register: ' + '\033[0;30;47m' + binaryRegister[:32] + '\033[0m' + binaryRegister[32:])
    
    for i in range(0, 32 - len(bin(Divisor).lstrip('0b')) + 1):
        if binaryDivisor <= binaryRegister[:32] :
            print('__________________________________________________________________________________')
            print('\033[91m Divisor is less then Dividend')
            print('Last bit in quotient is 1')
            print('Shifting register to left \033[0m')
            res = binarySubtract(binaryRegister[:32], binaryDivisor)
            binaryRegister = res + binaryRegister[32:]
            lsb = '1'
            print('__________________________________________________________________________________')
        else:
            print('__________________________________________________________________________________')
            print('\033[91mDivisor is greater then Dividend')
            print('The last quotient bit is set to 0 \033[0m')
            lsb = '0'
            print('__________________________________________________________________________________')
        binaryRegister = binaryRegister[:32] + shiftLeft(binaryRegister[32:])
        binaryRegister = binaryRegister[:63] + lsb
        binaryDivisor = shiftRight(binaryDivisor)
        print('Divisor:  ' + binaryDivisor)
        print('Register: ' + '\033[0;30;47m' + binaryRegister[:32] + '\033[0m' + binaryRegister[32:])

    return int(binaryRegister[32:], 2)


def binarySubtract(a, b, n=32):
    a_i = int(a, 2)
    b_i = int(b, 2)
    return shiftToNBit(bin(a_i-b_i).lstrip('0b'), n)


def shiftToNBit(binaryNumber, n=32):
    tempStr = ''
    while len(tempStr + binaryNumber) < n:
        tempStr += '0'
    
    return tempStr + binaryNumber


def shiftLeft(str):
    return str[1:len(str)] + '0'


def shiftRight(str):
    return '0' + str[:len(str) - 1]


if __name__ == "__main__":
    main()