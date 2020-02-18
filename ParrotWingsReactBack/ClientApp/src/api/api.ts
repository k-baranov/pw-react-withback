import axios, { AxiosInstance } from 'axios';
import { toast } from 'react-toastify';

import SessionApi from './sessionApi';
import TransactionApi from './transactionApi';

const defaultErrorMessage = 'Internal server error';

interface IResponseData {
  errorMessage?: string;
  errors?: any;
}

export function toastResponseErrors(response: IResponseData) {
  if (!response) {
    toast.error(defaultErrorMessage);
    return;
  }
  if (response.errors !== undefined) {
    for (const error in response.errors) {
      if (response.errors.hasOwnProperty(error)) {
        for (const message of (response.errors[error] as Array<string>)) {
          toast.error(message);
        }
      }
    }
    return;
  }
  if (response.errorMessage !== undefined) {
    toast.error(response.errorMessage);
    return;
  }
  toast.error(defaultErrorMessage);
}

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
