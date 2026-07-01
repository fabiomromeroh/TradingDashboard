import { toSelectOptions } from "@/lib/utils";
import { useAppDispatch, useAppSelector } from "@/store/hooks";
import { setSelectedAccounts } from "@/store/store";
import { AppMultiSelect } from "../shared/AppMultiSelect";

//function to add filters like Account and Data Range
export function FilterZone() {
  const accounts = useAppSelector((x) => x.account.accounts);
  const accountOptions = toSelectOptions(accounts);
  const dispatch = useAppDispatch();
  const selectedAccount = useAppSelector((x) => x.account.selectedAccounts);
  console.log(selectedAccount);

  return (
    <div className="w-[320px]">
      <AppMultiSelect
        options={accountOptions}
        value={selectedAccount}
        onValueChange={(values) => {
          console.log("onValueChange", values);

          dispatch(setSelectedAccounts(values));
        }}
        placeholder="Select Account"
        maxDisplayedItems={2}
      />
    </div>
  );
}
