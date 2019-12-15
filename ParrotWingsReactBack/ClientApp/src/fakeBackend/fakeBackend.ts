import { ITransactionInfo, ILoginOptions, IUserInfo, ISignUpOptions, ISessionInfo, INewTransaction } from "./backendModels";
import * as usersAsset from './usersAsset.json'

const INITIAL_BALANCE = 500;

export default class FakeBackend {
  private registredUsers: Array<IUserInfo> = new Array<IUserInfo>();
  private currentUserName: string | null = null;
  private transactions = new Map<string, Array<ITransactionInfo>>();

  constructor() {
    this.registredUsers.push(...usersAsset.users);
    
    for (const user of this.registredUsers) {
      this.transactions.set(user.username, new Array<ITransactionInfo>());
    }
  }

  async login(options: ILoginOptions): Promise<ISessionInfo> {
    const userInfo = this.findUserByEmail(options.email);
    if (userInfo !== undefined && userInfo.password === options.password) {
      this.currentUserName = userInfo.username;
      return Promise.resolve({ ...userInfo });
    }
    return Promise.reject(new Error('Invalid login or password'));
  }

  async signUp(options: ISignUpOptions): Promise<ISessionInfo> {
    if (this.findUserByEmail(options.email) !== undefined) {
      return Promise.reject(new Error(`Email '${options.email}' is already registred`));
    }
    if (this.findUserByName(options.username) !== undefined) {
      return Promise.reject(new Error(`Username '${options.username}' is already registred`));
    }
    this.registredUsers.push({ ...options, balance: INITIAL_BALANCE });
    this.transactions.set(options.username, new Array<ITransactionInfo>());
    this.currentUserName = options.username;
    return Promise.resolve({ username: options.username, balance: INITIAL_BALANCE });
  }

  async logout(): Promise<void> {
    this.currentUserName = null;
    return Promise.resolve();
  }

  async getSessionInfo(): Promise<ISessionInfo> {
    if (this.currentUserName === null) {
      return Promise.reject(new Error('No active user detected'));
    }
    const userInfo = this.findUserByName(this.currentUserName);
    if (userInfo !== undefined) {
      return Promise.resolve({ ...userInfo });
    }
    return Promise.reject(new Error('No active user detected'));
  }

  async getUsernameOptions(): Promise<Array<string>> {
    if (this.currentUserName === null) {
      return Promise.reject(new Error('No active user detected'));
    }
    
    // Exclude current user
    const users = this.registredUsers.filter((user) => user.username !== this.currentUserName);
    return Promise.resolve(users.map((user) => user.username));
  }

  async newTransaction(newTransaction: INewTransaction): Promise<void> {
    if (this.currentUserName === newTransaction.username) {
      return Promise.reject(new Error('PW cannot be sent to yourself'));
    }

    if (this.currentUserName === null) {
      return Promise.reject(new Error('No active user detected'));
    }

    const userInfo = this.findUserByName(this.currentUserName);
    if (userInfo !== undefined) {
      if (userInfo.balance < newTransaction.amount) {
        return Promise.reject(new Error('Not enough PW to remit the transaction'));
      }

      const recipientUserInfo = this.findUserByName(newTransaction.username);
      if (recipientUserInfo === undefined) {
        return Promise.reject(new Error(`User with name '${newTransaction.username}' does not exist`));
      }

      userInfo.balance -= newTransaction.amount;
      recipientUserInfo.balance += newTransaction.amount;

      const currentDate = new Date();  
      const currentUserTransactions = this.transactions.get(userInfo.username);
      const recipientUserTransactions = this.transactions.get(recipientUserInfo.username);

      if (currentUserTransactions !== undefined && recipientUserTransactions !== undefined) {
        const outcomeTransaction: ITransactionInfo = {
          id: currentUserTransactions.length,
          date: currentDate,
          correspondentName: recipientUserInfo.username,
          amount: -newTransaction.amount,
          resultBalance: userInfo.balance
        }
        currentUserTransactions.push(outcomeTransaction);

        const incomeTransaction: ITransactionInfo = {
          id: recipientUserTransactions.length,
          date: currentDate,
          correspondentName: userInfo.username,
          amount: newTransaction.amount,
          resultBalance: recipientUserInfo.balance
        }
        recipientUserTransactions.push(incomeTransaction);
      }
    }

    return Promise.resolve();
  }

  async getTransactions(): Promise<Array<ITransactionInfo>> {
    if (this.currentUserName === null) {
      return Promise.reject(new Error('No active user detected'));
    }

    return Promise.resolve(this.transactions.get(this.currentUserName)!);
  }

  private findUserByEmail(email: string): IUserInfo | undefined {
    const userInfo = this.registredUsers.find((user) => user.email === email);
    return userInfo;
  }

  private findUserByName(name: string): IUserInfo | undefined {
    const userInfo = this.registredUsers.find((user) => user.username === name);
    return userInfo;
  }
}