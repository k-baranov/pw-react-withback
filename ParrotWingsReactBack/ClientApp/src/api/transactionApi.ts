import { AxiosInstance } from 'axios';

import { ITransactionInfo, INewTransaction } from '../fakeBackend/backendModels';
import FakeBackend from '../fakeBackend/fakeBackend';

const TRANSACTION_LIST_URL = '/api/transactions/getAllForCurrentUser';
const NEW_TRANSACTION_URL = '/api/transactions/create';

export default class TransactionApi {
  constructor(private readonly client: AxiosInstance) {}
   
  async newTransaction(newTransaction: INewTransaction): Promise<void> {
    await this.client.post(NEW_TRANSACTION_URL, newTransaction);

    //await this.backend.newTransaction(newTransaction);
  }

  async getTransactions(): Promise<Array<ITransactionInfo>> {
    const { data } = await this.client.get(TRANSACTION_LIST_URL);
    return data;

    //const data = await this.backend.getTransactions();
    //return data;
  }
}