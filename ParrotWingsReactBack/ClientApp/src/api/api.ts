import SessionApi from './sessionApi';
import TransactionApi from './transactionApi';
import FakeBackend from '../fakeBackend/fakeBackend';

export default class Api {    
  public readonly session: SessionApi;
  public readonly transaction: TransactionApi;
  
  private backend: FakeBackend;

  constructor() {
    this.backend = new FakeBackend();    

    this.session = new SessionApi(this.backend);
    this.transaction = new TransactionApi(this.backend);
  }  
}
