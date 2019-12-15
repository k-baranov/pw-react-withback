import React, { useContext, useState, ChangeEvent } from 'react'
import { Button, Form, Grid, Header, Segment } from 'semantic-ui-react'
import { toast } from 'react-toastify';

import styles from './SignUpForm.module.css'
import { useHistory } from 'react-router';
import { SessionContext } from '../SessionProvider/SessionProvider';
import { NavRoute } from '../MainRouter/MainRouter';

export default function SignUpForm() {
  const history = useHistory();
  const { signUp } = useContext(SessionContext);
  const [ email, setEmail ] = useState('');
  const [ username, setUsername ] = useState('');
  const [ password, setPassword ] = useState('');    
  const [ confirmPassword, setConfirmPassword ] = useState('');

  const handleSignUpClick = async () => {
    try {
      await signUp({email, username, password, confirmPassword});
      history.push(NavRoute.Home);
    } catch (ex) {
      toast.error(ex.message);
    }
  }

  return (
    <Grid textAlign='center' className={styles.formGrid} verticalAlign='middle'>
      <Grid.Column className={styles.formGridColumn}>
        <Header as='h2' textAlign='center'>
          Sign Up
        </Header>
        <Form size='large'>
          <Segment stacked>
            <Form.Input 
              fluid 
              iconPosition='left' 
              placeholder='E-mail address' 
              value={email}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setEmail(e.target.value)}
            />
            <Form.Input 
              fluid 
              iconPosition='left' 
              placeholder='Username'
              value={username}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setUsername(e.target.value)} 
            />
            <Form.Input
              fluid
              iconPosition='left'
              placeholder='Password'
              type='password'
              value={password}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setPassword(e.target.value)}
            />
            <Form.Input
              fluid
              iconPosition='left'
              placeholder='Confirm password'
              type='password'
              value={confirmPassword}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setConfirmPassword(e.target.value)}
            />

            <Button primary fluid size='large' onClick={handleSignUpClick}>
              Sign Up
            </Button>
          </Segment>
        </Form>
      </Grid.Column>
    </Grid>
  )
} 