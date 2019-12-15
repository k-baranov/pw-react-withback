export interface ITransactionInfo {
  id: number;
  date: Date;
  correspondentName: string;
  amount: number;
  resultBalance: number;
}

export interface INewTransaction {
  username: string;
  amount: number;  
}

export interface ISessionInfo {
  username: string;
  balance: number;
}

export interface ILoginOptions {
  email: string;
  password: string;  
}

export interface ISignUpOptions {
  email: string;
  username: string;
  password: string;
  confirmPassword: string;
}

export interface IUserInfo {
  email: string;
  username: string;
  password: string;
  balance: number;
}

