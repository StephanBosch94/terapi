/* eslint-disable no-console */

class Logger {
  static info(...args: any[]): void {
    console.log(...args);
  }

  static warn(...args: any[]): void {
    console.warn(...args);
  }

  static error(...args: any[]): void {
    console.error(...args);
  }
}

export default Logger;
