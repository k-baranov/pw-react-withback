import React, { useEffect, useContext, useState } from 'react'
import { Table, Button, Icon } from 'semantic-ui-react'
import { toast } from 'react-toastify';

import { ApiContext } from '../ApiProvider/ApiProvider'
import { useHistory } from 'react-router';
import { NavRoute } from '../MainRouter/MainRouter';
import { ITransactionInfo } from '../../models/backendModels';
import { INavProps } from '../Nav/Nav'

export default function TransactionHistory({setActiveNavItem}: INavProps) {
  const history = useHistory();    
  const [ transactions, setTransactions ] = useState(new Array<ITransactionInfo>());
  const api = useContext(ApiContext);

  const fetchTransactions = async () => {
    try {
      const data = await api.transaction.getTransactions();
      setTransactions(data);
    } catch (ex) {
      toast.error(ex.message);
    }
  }

  useEffect(() => {
    fetchTransactions(); 
    // eslint-disable-next-line react-hooks/exhaustive-deps       
  }, [])

  const handleCopyClick = (transaction: ITransactionInfo) => {
    const absAmount = Math.abs(transaction.amount);
    setActiveNavItem!(NavRoute.TransNew);
    history.push(`${NavRoute.TransNew}?username=${transaction.correspondentName}&amount=${absAmount}`)
  }

  return (
    <Table celled>
      <Table.Header>
      <Table.Row>
        <Table.HeaderCell>Date</Table.HeaderCell>
        <Table.HeaderCell>Correspondent name</Table.HeaderCell>
        <Table.HeaderCell>Amount</Table.HeaderCell>
        <Table.HeaderCell>Balance</Table.HeaderCell>
        <Table.HeaderCell />
      </Table.Row>
      </Table.Header>

      
      <Table.Body>
      {transactions.map((transaction: ITransactionInfo) => 
        <Table.Row key={transaction.date}>
          <Table.Cell>{transaction.date}</Table.Cell>
          <Table.Cell>{transaction.correspondentName}</Table.Cell>
          <Table.Cell>{transaction.amount}</Table.Cell>
          <Table.Cell>{transaction.resultBalance}</Table.Cell>
          <Table.Cell>
            {transaction.amount < 0 && 
              <Button icon labelPosition='left' onClick={() => handleCopyClick(transaction)}>
                <Icon name='copy outline' />
                Copy
              </Button>}
          </Table.Cell>
        </Table.Row>)
      }
      </Table.Body>
    </Table>
  )
}