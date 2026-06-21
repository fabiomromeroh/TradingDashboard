import { DataTable } from '@/components/shared/DataTable';
import type { Account } from "../types/account.types";
import type { ColumnDef } from '@tanstack/react-table';
import { DataTableActions } from '@/components/shared/DataTableActions';
import { useAccounts } from '../hooks/useAccounts';
import { CreateAccountModal } from './CreateAccountModal';


const columns: ColumnDef<Account, unknown>[] = [
    { accessorKey: 'name', header: 'Account Name' },
    { accessorKey: 'brokerName', header: 'Broker' },
    { accessorKey: 'currency', header: 'Currency' },
    { accessorKey: 'Trades', header: 'Trades' },
  {
    id: 'actions',
    enableHiding: false,
    cell: ({ row }) => {
      const account = row.original;
      return (
        <DataTableActions
          entity={account}
          actions={[{ label: 'Edit', onClick: () => {} }]}
        />
      );
    },
  },
];

export function AccountTable() {

  const { accounts } = useAccounts();

  return  <DataTable columns={columns} data={accounts} showFilter={false} toolbar={<CreateAccountModal />}/>
  
  
}