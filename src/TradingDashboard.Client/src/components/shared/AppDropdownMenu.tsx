import * as React from "react";
import {
  DropdownMenu,
  DropdownMenuCheckboxItem,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuPortal,
  DropdownMenuRadioGroup,
  DropdownMenuRadioItem,
  DropdownMenuSeparator,
  DropdownMenuShortcut,
  DropdownMenuSub,
  DropdownMenuSubContent,
  DropdownMenuSubTrigger,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { AppButton } from "./AppButton";

type Align = "start" | "center" | "end";

type CommonMenuProps = {
  trigger?: React.ReactNode;
  label?: string;
  align?: Align;
  sideOffset?: number;
  className?: string;
};

type BaseEntry = {
  label: string;
  disabled?: boolean;
  shortcut?: string;
  icon?: React.ReactNode;
};

type ActionEntry = BaseEntry & {
  value: string;
  onSelect?: (value: string) => void;
  destructive?: boolean;
};

type CheckboxEntry = BaseEntry & {
  value: string;
};

type RadioEntry = BaseEntry & {
  value: string;
};

type SubmenuEntry = BaseEntry & {
  items: ActionEntry[];
};

type MixedEntry =
  | { kind: "label"; label: string }
  | { kind: "separator" }
  | ({ kind: "item" } & ActionEntry)
  | ({ kind: "checkbox" } & CheckboxEntry)
  | ({ kind: "submenu" } & SubmenuEntry);

type ItemsVariantProps = CommonMenuProps & {
  variant: "items";
  items: ActionEntry[];
};

type CheckboxVariantProps = CommonMenuProps & {
  variant: "checkbox";
  items: CheckboxEntry[];
  value: string[];
  onValueChange: (value: string[]) => void;
};

type RadioVariantProps = CommonMenuProps & {
  variant: "radio";
  items: RadioEntry[];
  value: string;
  onValueChange: (value: string) => void;
};

type SubmenuVariantProps = CommonMenuProps & {
  variant: "submenu";
  items: (ActionEntry | SubmenuEntry)[];
};

type MixedVariantProps = CommonMenuProps & {
  variant: "mixed";
  items: MixedEntry[];
  checkboxValue?: string[];
  onCheckboxValueChange?: (value: string[]) => void;
};

export type AppDropdownMenuProps =
  | ItemsVariantProps
  | CheckboxVariantProps
  | RadioVariantProps
  | SubmenuVariantProps
  | MixedVariantProps;

function isSubmenuEntry(
  item: ActionEntry | SubmenuEntry,
): item is SubmenuEntry {
  return "items" in item;
}

function renderShortcut(shortcut?: string) {
  if (!shortcut) return null;
  return <DropdownMenuShortcut>{shortcut}</DropdownMenuShortcut>;
}

export function AppDropdownMenu(props: AppDropdownMenuProps) {
  const trigger = props.trigger ?? (
    <AppButton variant="outline">{props.label}</AppButton>
  );

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>{trigger}</DropdownMenuTrigger>

      <DropdownMenuContent
        align={props.align ?? "start"}
        sideOffset={props.sideOffset ?? 8}
        className={props.className ?? "min-w-56"}
      >
        {props.label ? (
          <>
            <DropdownMenuLabel>{props.label}</DropdownMenuLabel>
            <DropdownMenuSeparator />
          </>
        ) : null}

        {props.variant === "items" && (
          <DropdownMenuGroup>
            {props.items.map((item) => (
              <DropdownMenuItem
                key={item.value}
                disabled={item.disabled}
                onSelect={() => item.onSelect?.(item.value)}
                className={
                  item.destructive
                    ? "text-destructive focus:text-destructive"
                    : undefined
                }
              >
                {item.icon}
                <span>{item.label}</span>
                {renderShortcut(item.shortcut)}
              </DropdownMenuItem>
            ))}
          </DropdownMenuGroup>
        )}

        {props.variant === "checkbox" && (
          <DropdownMenuGroup>
            {props.items.map((item) => {
              const checked = props.value.includes(item.value);

              return (
                <DropdownMenuCheckboxItem
                  key={item.value}
                  checked={checked}
                  disabled={item.disabled}
                  onCheckedChange={(next) => {
                    if (next) {
                      props.onValueChange([...props.value, item.value]);
                    } else {
                      props.onValueChange(
                        props.value.filter((v) => v !== item.value),
                      );
                    }
                  }}
                >
                  {item.icon}
                  <span>{item.label}</span>
                  {renderShortcut(item.shortcut)}
                </DropdownMenuCheckboxItem>
              );
            })}
          </DropdownMenuGroup>
        )}

        {props.variant === "radio" && (
          <DropdownMenuRadioGroup
            value={props.value}
            onValueChange={props.onValueChange}
          >
            {props.items.map((item) => (
              <DropdownMenuRadioItem
                key={item.value}
                value={item.value}
                disabled={item.disabled}
              >
                {item.icon}
                <span>{item.label}</span>
                {renderShortcut(item.shortcut)}
              </DropdownMenuRadioItem>
            ))}
          </DropdownMenuRadioGroup>
        )}

        {props.variant === "submenu" && (
          <DropdownMenuGroup>
            {props.items.map((item, index) =>
              isSubmenuEntry(item) ? (
                <DropdownMenuSub key={`${item.label}-${index}`}>
                  <DropdownMenuSubTrigger>
                    {item.icon}
                    <span>{item.label}</span>
                  </DropdownMenuSubTrigger>

                  <DropdownMenuPortal>
                    <DropdownMenuSubContent>
                      {item.items.map((subItem) => (
                        <DropdownMenuItem
                          key={subItem.value}
                          disabled={subItem.disabled}
                          onSelect={() => subItem.onSelect?.(subItem.value)}
                          className={
                            subItem.destructive
                              ? "text-destructive focus:text-destructive"
                              : undefined
                          }
                        >
                          {subItem.icon}
                          <span>{subItem.label}</span>
                          {renderShortcut(subItem.shortcut)}
                        </DropdownMenuItem>
                      ))}
                    </DropdownMenuSubContent>
                  </DropdownMenuPortal>
                </DropdownMenuSub>
              ) : (
                <DropdownMenuItem
                  key={item.value}
                  disabled={item.disabled}
                  onSelect={() => item.onSelect?.(item.value)}
                  className={
                    item.destructive
                      ? "text-destructive focus:text-destructive"
                      : undefined
                  }
                >
                  {item.icon}
                  <span>{item.label}</span>
                  {renderShortcut(item.shortcut)}
                </DropdownMenuItem>
              ),
            )}
          </DropdownMenuGroup>
        )}

        {props.variant === "mixed" && (
          <DropdownMenuGroup>
            {props.items.map((item, index) => {
              if (item.kind === "separator") {
                return <DropdownMenuSeparator key={`separator-${index}`} />;
              }

              if (item.kind === "label") {
                return (
                  <DropdownMenuLabel key={`label-${index}`}>
                    {item.label}
                  </DropdownMenuLabel>
                );
              }

              if (item.kind === "item") {
                return (
                  <DropdownMenuItem
                    key={item.value}
                    disabled={item.disabled}
                    onSelect={() => item.onSelect?.(item.value)}
                    className={
                      item.destructive
                        ? "text-destructive focus:text-destructive"
                        : undefined
                    }
                  >
                    {item.icon}
                    <span>{item.label}</span>
                    {renderShortcut(item.shortcut)}
                  </DropdownMenuItem>
                );
              }

              if (item.kind === "checkbox") {
                const values = props.checkboxValue ?? [];
                const checked = values.includes(item.value);

                return (
                  <DropdownMenuCheckboxItem
                    key={item.value}
                    checked={checked}
                    disabled={item.disabled}
                    onCheckedChange={(next) => {
                      if (!props.onCheckboxValueChange) return;

                      if (next) {
                        props.onCheckboxValueChange([...values, item.value]);
                      } else {
                        props.onCheckboxValueChange(
                          values.filter((v) => v !== item.value),
                        );
                      }
                    }}
                  >
                    {item.icon}
                    <span>{item.label}</span>
                    {renderShortcut(item.shortcut)}
                  </DropdownMenuCheckboxItem>
                );
              }

              if (item.kind === "submenu") {
                return (
                  <DropdownMenuSub key={`${item.label}-${index}`}>
                    <DropdownMenuSubTrigger>
                      {item.icon}
                      <span>{item.label}</span>
                    </DropdownMenuSubTrigger>

                    <DropdownMenuPortal>
                      <DropdownMenuSubContent>
                        {item.items.map((subItem) => (
                          <DropdownMenuItem
                            key={subItem.value}
                            disabled={subItem.disabled}
                            onSelect={() => subItem.onSelect?.(subItem.value)}
                            className={
                              subItem.destructive
                                ? "text-destructive focus:text-destructive"
                                : undefined
                            }
                          >
                            {subItem.icon}
                            <span>{subItem.label}</span>
                            {renderShortcut(subItem.shortcut)}
                          </DropdownMenuItem>
                        ))}
                      </DropdownMenuSubContent>
                    </DropdownMenuPortal>
                  </DropdownMenuSub>
                );
              }

              return null;
            })}
          </DropdownMenuGroup>
        )}
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
