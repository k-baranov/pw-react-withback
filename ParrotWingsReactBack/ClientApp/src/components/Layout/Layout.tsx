import React, { ReactElement } from 'react'
import { Container } from 'semantic-ui-react'
import Nav, { INavProps } from '../Nav/Nav'

export default function Layout({children, activeNavItem, setActiveNavItem} : {children: ReactElement} & INavProps) {  
  return (
    <>
      <Nav activeNavItem={activeNavItem} setActiveNavItem={setActiveNavItem}/>
      <Container text>
        {React.cloneElement(children, { setActiveNavItem })}
      </Container>
    </>
  )
}