//
// Created by galex on 3/10/2022.
//

module;

#ifndef _WIN32
#include <cstdio>
#include <termios.h>
#include <unistd.h>
#include <fcntl.h>
#else
#include <conio.h>
#endif

export module wait;

#ifndef _WIN32
export int keyboardHit() {
  struct termios oldt, newt;
  int ch;
  int oldf;

  tcgetattr(STDIN_FILENO, &oldt);
  newt = oldt;
  newt.c_lflag &= ~(ICANON | ECHO);
  tcsetattr(STDIN_FILENO, TCSANOW, &newt);
  oldf = fcntl(STDIN_FILENO, F_GETFL, 0);
  fcntl(STDIN_FILENO, F_SETFL, oldf | O_NONBLOCK);

  ch = getchar();

  tcsetattr(STDIN_FILENO, TCSANOW, &oldt);
  fcntl(STDIN_FILENO, F_SETFL, oldf);

  if (ch != EOF) {
    ungetc(ch, stdin);
    return 1;
  }

  return 0;
}
#else
export int keyboardHit() {
  return _kbhit();
}
#endif

export void wait() {
  while (!keyboardHit());
}