import React, { useContext } from 'react'
import { useHistory } from 'react-router-dom';
import {Menu, Button, Icon} from 'semantic-ui-react'

import UserBalance from '../UserBalance/UserBalance';
import { NavRoute } from '../MainRouter/MainRouter'
import { SessionContext } from '../SessionProvider/SessionProvider';

export interface INavProps {
  activeNavItem?: NavRoute, 
  setActiveNavItem: React.Dispatch<React.SetStateAction<NavRoute>>
}

export default function Nav({activeNavItem, setActiveNavItem}: INavProps) {
  const { logout } = useContext(SessionContext);  
  const history = useHistory();

  function handleMenuClick(route: NavRoute) {
    setActiveNavItem(route);
    history.push(route)    
  }

  return (
    <Menu>
      <Menu.Item header>
        <Icon name='money bill alternate' size='big'/>
        Parrot Wings
      </Menu.Item>
      <Menu.Item 
        name='Home'
        active={activeNavItem === NavRoute.Home}
        onClick={() => handleMenuClick(NavRoute.Home)}
      />
      <Menu.Item 
        name='Transactions'
        active={activeNavItem === NavRoute.TransHistory}
        onClick={() => handleMenuClick(NavRoute.TransHistory)}
      />
      <Menu.Item 
        name='New transaction'
        active={activeNavItem === NavRoute.TransNew}
        onClick={() => handleMenuClick(NavRoute.TransNew)}
      />
      <UserBalance />
      <Menu.Item>
        <Button secondary as='a' onClick={logout}>
          Logout
        </Button>
      </Menu.Item>
    </Menu>
  );
}