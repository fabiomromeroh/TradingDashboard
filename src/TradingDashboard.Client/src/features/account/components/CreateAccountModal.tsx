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
import { zodResolver } from "@hookform/resolvers/zod";
import { useState } from "react";
import { useForm } from "react-hook-form";
import z from "zod";

const accountSchema = z.object({
  name: z.string().min(1, "Account name is required"),
  broker: z.string().min(1, "Broker is required"),
  currency: z.string().min(1, "Currency is required"),
});

type CreateAccountFormValues = z.infer<typeof accountSchema>;

export function CreateAccountModal() {
  const [open, setOpen] = useState(false);
  const form = useForm<CreateAccountFormValues>({
    resolver: zodResolver(accountSchema),
    defaultValues: { name: "", broker: "", currency: "" },
  });

  async function handleFormSubmit(values: CreateAccountFormValues) {
    void values;
    // Handle form submission logic here
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
              options={[
                { value: "broker1", label: "Broker 1" },
                { value: "broker2", label: "Broker 2" },
              ]}
            />

            <AppSelectField
              name="currency"
              control={form.control}
              label="Currency"
              placeholder="Select a currency"
              options={[
                { value: "USD", label: "USD" },
                { value: "EUR", label: "EUR" },
              ]}
            />
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
