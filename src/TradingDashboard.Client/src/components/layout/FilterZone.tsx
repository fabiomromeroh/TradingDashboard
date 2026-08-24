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
  const filtersConfig = useAppSelector((x) => x.auth.filtersConfig);

  return (
    <div className="w-[320px]">
      <AppMultiSelect
        options={accountOptions}
        value={filtersConfig.accountIds}
        onValueChange={(values) => {
          updateConfigFilters(
            { ...filtersConfig, accountIds: values },
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
