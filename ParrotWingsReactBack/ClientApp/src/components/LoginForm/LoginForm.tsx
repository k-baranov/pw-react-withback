import React, { useContext, useState, ChangeEvent } from 'react'
import { Link, useHistory } from 'react-router-dom'
import { Button, Form, Grid, Header, Message, Segment } from 'semantic-ui-react'
import { toast } from 'react-toastify';

import styles from './LoginForm.module.css'
import { SessionContext } from '../SessionProvider/SessionProvider';
import { NavRoute } from '../MainRouter/MainRouter';

export default function LoginForm() {
  const history = useHistory();
  const { login } = useContext(SessionContext);
  const [ email, setEmail ] = useState('');
  const [ password, setPassword ] = useState('');

  const handleLoginClick = async () => {
    try {
      await login({email, password});
      history.push(NavRoute.Home);
    } catch (ex) {
      toast.error(ex.message);
    }
  }

  return (
    <Grid textAlign='center' className={styles.formGrid} verticalAlign='middle'>
      <Grid.Column className={styles.formGridColumn}>
        <Header as='h2' textAlign='center'>
          Log-in to your account
        </Header>
        <Form size='large'>
          <Segment stacked>
            <Form.Input 
              fluid 
              icon='user' 
              iconPosition='left' 
              placeholder='E-mail address' 
              value={email}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setEmail(e.target.value)}
            />
            <Form.Input
              fluid
              icon='lock'
              iconPosition='left'
              placeholder='Password'
              type='password'
              value={password}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setPassword(e.target.value)}
            />

            <Button primary fluid size='large' onClick={handleLoginClick}>
              Login
            </Button>
          </Segment>
        </Form>
        <Message>
          New to us? <Link to={NavRoute.SignUp}>Sign Up</Link>
        </Message>
      </Grid.Column>
    </Grid>
  )
} 