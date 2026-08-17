import { AppInputField } from "@/components/shared/AppInputField";
import { AppSelectField } from "@/components/shared/AppSelectField";
import type { CreateAccountModalProps } from "../types/account.types";
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { Form } from "@/components/ui/form";
import { useBrokersQuery } from "@/features/import/hooks/useBrokersQuery";
import { zodResolver } from "@hookform/resolvers/zod";
import { useState } from "react";
import { useForm } from "react-hook-form";
import z from "zod";
import { useCreateAccountMutation } from "../hooks/useCreateAccountMutation";
import { toast } from "sonner";
import { AppButton } from "@/components/shared/AppButton";

const accountSchema = z.object({
  name: z.string().min(1, "Account name is required"),
  broker: z.string().min(1, "Broker is required"),
  type: z.string().min(1, "Type is required"),
});

type CreateAccountFormValues = z.infer<typeof accountSchema>;

export function CreateAccountModal(props: CreateAccountModalProps) {
  const [open, setOpen] = useState(false);
  const form = useForm<CreateAccountFormValues>({
    resolver: zodResolver(accountSchema),
    defaultValues: { name: "", broker: "", type: "" },
  });

  const { brokers } = useBrokersQuery();
  const { mutate: createAccount } = useCreateAccountMutation();

  async function handleFormSubmit(values: CreateAccountFormValues) {
    const success = await createAccount({
      name: values.name,
      importSourceType: values.type,
      brokerId: values.broker,
      initialBalance: 0, // Set initial balance to 0
    });

    if (success) {
      toast.success("Account created successfully");
      setOpen(false);
      form.reset();
      props.handleOnAccountChange();
    }
    setOpen(false);
  }

  return (
    <Form {...form}>
      <Dialog open={open} onOpenChange={setOpen}>
        <DialogTrigger asChild>
          <AppButton className="float-right" variant="outline">
            Add Account
          </AppButton>
        </DialogTrigger>
        <DialogContent className="sm:max-w-sm">
          <DialogHeader>
            <DialogTitle>Add Account</DialogTitle>
            <DialogDescription>Add a new brokerage account</DialogDescription>
          </DialogHeader>
          <form
            onSubmit={form.handleSubmit(handleFormSubmit)}
            className="space-y-4"
          >
            <div className="grid grid-cols-1 gap-4">
              <AppInputField
                name="name"
                control={form.control}
                placeholder="Account Name"
                label="Name"
              />
              <AppSelectField
                name="broker"
                control={form.control}
                placeholder="Select a broker"
                label="Broker"
                className="w-full"
                options={brokers}
              />

              <AppSelectField
                name="type"
                control={form.control}
                label="Type"
                placeholder="Select a type"
                className="w-full"
                options={[
                  { value: "FileUpload", label: "File Upload" },
                  { value: "BrokerSync", label: "Broker Sync" },
                  { value: "ManualEntry", label: "Manual" },
                ]}
              />
            </div>
            <DialogFooter>
              <DialogClose asChild>
                <AppButton variant="outline" type="button">
                  Cancel
                </AppButton>
              </DialogClose>
              <AppButton type="submit">Save</AppButton>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog>
    </Form>
  );
}
