import React, { useState } from 'react'
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";

import Layout from '../Layout/Layout';
import LoginForm from '../LoginForm/LoginForm';
import PrivateRoute from '../PrivateRoute/PrivateRoute';
import Home from '../Home/Home';
import TransactionHistory from '../TransactionHistory/TransactionHistory';
import NewTransaction from '../NewTransaction/NewTransaction';
import NotFound from '../NotFound/NotFound';
import SignUpForm from '../SignUpForm/SignUpForm';

export enum NavRoute {
  Login = '/login',
  SignUp = '/signUp',
  Home = '/',
  TransHistory = '/transaction/history',
  TransNew = '/transaction/new',
  NotFound = '*'
}

export default function MainRouter() {
  const [activeNavItem, setActiveNavItem] = useState(NavRoute.Home);
  
  return (
    <Router>
      <Switch>
        <Route path={NavRoute.Login}>
          <LoginForm setActiveNavItem={setActiveNavItem}/>
        </Route>
        <Route path={NavRoute.SignUp}>
          <SignUpForm setActiveNavItem={setActiveNavItem}/>
        </Route>
        <PrivateRoute exact path={NavRoute.Home}>
          <Layout activeNavItem={activeNavItem} setActiveNavItem={setActiveNavItem}>
            <Home/>
          </Layout>
        </PrivateRoute>
        <PrivateRoute path={NavRoute.TransHistory}>
          <Layout activeNavItem={activeNavItem} setActiveNavItem={setActiveNavItem}>
            <TransactionHistory setActiveNavItem={setActiveNavItem}/>
          </Layout>
        </PrivateRoute>
        <PrivateRoute path={NavRoute.TransNew}>
          <Layout activeNavItem={activeNavItem} setActiveNavItem={setActiveNavItem}>
            <NewTransaction/>
          </Layout>
        </PrivateRoute>
        <PrivateRoute path={NavRoute.NotFound}>
          <Layout activeNavItem={activeNavItem} setActiveNavItem={setActiveNavItem}>
            <NotFound/>
          </Layout>
        </PrivateRoute>
      </Switch>
    </Router>
  )
}