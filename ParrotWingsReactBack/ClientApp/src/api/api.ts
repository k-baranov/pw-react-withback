import axios, { AxiosInstance } from 'axios';

import SessionApi from './sessionApi';
import TransactionApi from './transactionApi';

export default class Api {    
  public readonly session: SessionApi;
  public readonly transaction: TransactionApi;
  
  private client: AxiosInstance;

  constructor() {
    this.client = axios.create();

    this.session = new SessionApi(this.client);
    this.transaction = new TransactionApi(this.client);
  }  
}
