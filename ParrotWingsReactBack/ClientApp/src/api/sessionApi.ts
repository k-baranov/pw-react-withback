import { ILoginOptions, ISignUpOptions, ISessionInfo } from '../fakeBackend/backendModels';
import FakeBackend from '../fakeBackend/fakeBackend';

//const LOGIN_URL = '/api/session/login';
//const SINGUP_URL = '/api/session/signUp';
//const LOGOUT_URL = '/api/session/logout';
//const SESSION_INFO_URL = '/api/session/getSessionInfo';

export default class SessionApi {
  constructor(private readonly backend: FakeBackend) {}

  async login(options: ILoginOptions): Promise<ISessionInfo> {
    //await this.client.post(LOGIN_URL, options);
    
    const data = await this.backend.login(options);
    return data;
  }

  async signUp(options: ISignUpOptions): Promise<ISessionInfo> {
    //await this.client.post(SINGUP_URL, options);
    
    const data = await this.backend.signUp(options);
    return data;
  }

  async logout(): Promise<void> {
    //await this.client.post(LOGOUT_URL);

    await this.backend.logout();
  }

  async getSessionInfo(): Promise<ISessionInfo> {
    //const { data } = await this.client.get(SESSION_INFO_URL);
    //return { ...data };

    const data = await this.backend.getSessionInfo();
    return data;
  }

  async getUsernameOptions(): Promise<Array<string>> {
    const data = await this.backend.getUsernameOptions();
    return data;
  }
}