import { AppInputField } from "@/components/shared/AppInputField";
import { AppSelectField } from "@/components/shared/AppSelectField";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogClose,
  DialogContent,
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

const accountSchema = z.object({
  name: z.string().min(1, "Account name is required"),
  broker: z.string().min(1, "Broker is required"),
  currency: z.string().min(1, "Currency is required"),
});

type CreateAccountFormValues = z.infer<typeof accountSchema>;

type CreateAccountModalProps = {
  handleOnAccountChange: () => void;
};

export function CreateAccountModal(props: CreateAccountModalProps) {
  const [open, setOpen] = useState(false);
  const form = useForm<CreateAccountFormValues>({
    resolver: zodResolver(accountSchema),
    defaultValues: { name: "", broker: "", currency: "" },
  });

  const { brokers } = useBrokersQuery();
  const { mutate: createAccount, error } = useCreateAccountMutation();

  async function handleFormSubmit(values: CreateAccountFormValues) {
    const success = await createAccount({
      name: values.name,
      brokerId: values.broker,
      currency: values.currency,
      initialBalance: 0, // Set initial balance to 0
    });

    if (success) {
      toast.success("Account created successfully");
      setOpen(false);
      form.reset();
      props.handleOnAccountChange();
    } else {
      toast.error(error ?? "Failed to create account");
    }
    setOpen(false);
  }

  return (
    <Form {...form}>
      <Dialog open={open} onOpenChange={setOpen}>
        <DialogTrigger asChild>
          <Button className="float-right" variant="outline">
            Add Account
          </Button>
        </DialogTrigger>
        <DialogContent className="sm:max-w-sm">
          <DialogHeader>
            <DialogTitle>Add Account</DialogTitle>
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
                name="currency"
                control={form.control}
                label="Currency"
                placeholder="Select a currency"
                className="w-full"
                options={[
                  { value: "USD", label: "USD" },
                  { value: "EUR", label: "EUR" },
                ]}
              />
            </div>
            <DialogFooter>
              <DialogClose asChild>
                <Button variant="outline" type="button">
                  Cancel
                </Button>
              </DialogClose>
              <Button type="submit">Save</Button>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog>
    </Form>
  );
}
