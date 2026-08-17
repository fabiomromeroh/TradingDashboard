import { toSelectOptions } from "@/lib/utils";
import { useAppDispatch, useAppSelector } from "@/store/hooks";
import { setConfigFilters } from "@/store/store";
import { AppMultiSelect } from "../shared/AppMultiSelect";
import { useConfigFiltersMutation } from "@/features/users/hooks/useConfigFiltersMutation";

//function to add filters like Account and Data Range
export function FilterZone() {
  const accounts = useAppSelector((x) => x.account.accounts);
  const accountOptions = toSelectOptions(accounts);
  const dispatch = useAppDispatch();
  const { mutate: updateConfigFilters } = useConfigFiltersMutation();

  const configFilter = useAppSelector((x) => x.auth.configFilters);

  return (
    <div className="w-[320px]">
      <AppMultiSelect
        options={accountOptions}
        value={configFilter.accountIds}
        onValueChange={(values) => {
          updateConfigFilters(
            { AccountIds: values },
            {
              onSuccess: () => {
                dispatch(setConfigFilters({ accountIds: values }));
              },
            },
          );
        }}
        placeholder="Select Account"
        maxDisplayedItems={2}
      />
    </div>
  );
}
