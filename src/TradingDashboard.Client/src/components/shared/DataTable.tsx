import * as React from "react";
import {
  type ColumnDef,
  type ColumnFiltersState,
  type SortingState,
  type VisibilityState,
  flexRender,
  getCoreRowModel,
  getFilteredRowModel,
  getPaginationRowModel,
  getSortedRowModel,
  useReactTable,
} from "@tanstack/react-table";
import { ChevronDown, ChevronUp, ChevronsUpDown } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Checkbox } from "@/components/ui/checkbox";
import {
  DropdownMenu,
  DropdownMenuCheckboxItem,
  DropdownMenuContent,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Input } from "@/components/ui/input";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { DataTableDetails } from "./DataTableDetails";

interface DataTableProps<TData> {
  columns: ColumnDef<TData, unknown>[];
  data: TData[];
  filterPlaceholder?: string;
  withFilter?: boolean;
  toolbar?: React.ReactNode;
  withColumnVisibilityToggle?: boolean;
  withPagination?: boolean;
  pageSize?: number;
  withTradeFooter?: boolean;
  withViewDetails?: boolean;
  detailColumns?: ColumnDef<any, unknown>[];
  detailsFetcher?: (row: TData) => Promise<any[]>;
  detailTitle?: string;
  onLoadMore?: () => void;
  hasMore?: boolean;
  isLoadingMore?: boolean;
  totalCount?: number;
}

export function DataTable<TData>({
  columns,
  data,
  filterPlaceholder = "Filter...",
  toolbar,
  withColumnVisibilityToggle = false,
  withPagination = false,
  pageSize = 50,
  withTradeFooter = false,
  withViewDetails = false,
  withFilter = false,
  detailColumns = [],
  detailsFetcher,
  detailTitle = "Details",
  onLoadMore,
  hasMore = false,
  isLoadingMore = false,
  totalCount = 0,
}: DataTableProps<TData>) {
  const [sorting, setSorting] = React.useState<SortingState>([]);
  const [columnFilters, setColumnFilters] = React.useState<ColumnFiltersState>(
    [],
  );
  const [columnVisibility, setColumnVisibility] =
    React.useState<VisibilityState>({});
  const [rowSelection, setRowSelection] = React.useState({});
  const [globalFilter, setGlobalFilter] = React.useState("");
  const [expandedRowId, setExpandedRowId] = React.useState<string | null>(null);
  const [detailRowsById, setDetailRowsById] = React.useState<
    Record<string, any[]>
  >({});
  const [loadingRowId, setLoadingRowId] = React.useState<string | null>(null);
  const [detailErrorsById, setDetailErrorsById] = React.useState<
    Record<string, string>
  >({});

  const toggleRowDetails = React.useCallback(
    async (row: TData) => {
      if (!withViewDetails || !detailsFetcher) {
        return;
      }

      const rowId = String((row as { id?: string }).id ?? "");
      if (!rowId) {
        return;
      }

      const isOpening = expandedRowId !== rowId;
      setExpandedRowId(isOpening ? rowId : null);

      if (!isOpening || detailRowsById[rowId]) {
        return;
      }

      setLoadingRowId(rowId);
      setDetailErrorsById((prev) => {
        const next = { ...prev };
        delete next[rowId];
        return next;
      });

      try {
        const rows = await detailsFetcher(row);
        setDetailRowsById((prev) => ({ ...prev, [rowId]: rows }));
      } catch (error) {
        setDetailErrorsById((prev) => ({
          ...prev,
          [rowId]:
            error instanceof Error ? error.message : "Failed to load details.",
        }));
      } finally {
        setLoadingRowId(null);
      }
    },
    [detailRowsById, detailsFetcher, expandedRowId, withViewDetails],
  );

  const allColumns = React.useMemo<ColumnDef<TData, unknown>[]>(() => {
    const selectionColumn: ColumnDef<TData, unknown> = {
      id: "select",
      header: ({ table }) => (
        <Checkbox
          checked={
            table.getIsAllPageRowsSelected() ||
            (table.getIsSomePageRowsSelected() && "indeterminate")
          }
          onCheckedChange={(value) => table.toggleAllPageRowsSelected(!!value)}
          aria-label="Select all"
        />
      ),
      cell: ({ row }) => (
        <div
          className="flex items-center space-x-2"
          onClick={(event) => event.stopPropagation()}
        >
          <Checkbox
            checked={row.getIsSelected()}
            onCheckedChange={(value) => row.toggleSelected(!!value)}
            aria-label="Select row"
          />
        </div>
      ),
      enableSorting: false,
      enableHiding: false,
    };

    return [selectionColumn, ...columns];
  }, [columns]);

  const table = useReactTable({
    data,
    columns: allColumns,
    onSortingChange: setSorting,
    onColumnFiltersChange: setColumnFilters,
    getCoreRowModel: getCoreRowModel(),
    getPaginationRowModel: withPagination ? getPaginationRowModel() : undefined,
    getSortedRowModel: getSortedRowModel(),
    getFilteredRowModel: getFilteredRowModel(),
    onColumnVisibilityChange: setColumnVisibility,
    onRowSelectionChange: setRowSelection,
    onGlobalFilterChange: setGlobalFilter,
    initialState: {
      pagination: {
        pageIndex: 0,
        pageSize: pageSize,
      },
    },
    state: {
      sorting,
      columnFilters,
      columnVisibility,
      rowSelection,
      globalFilter,
    },
  });

  const scrollContainerRef = React.useRef<HTMLDivElement>(null);

  React.useEffect(() => {
    const scrollContainer = scrollContainerRef.current;
    if (!scrollContainer || withPagination || !onLoadMore) return;

    const handleScroll = () => {
      const { scrollTop, scrollHeight, clientHeight } = scrollContainer;
      const threshold = 200;

      if (
        scrollHeight - (scrollTop + clientHeight) < threshold &&
        hasMore &&
        !isLoadingMore
      ) {
        onLoadMore();
      }
    };

    scrollContainer.addEventListener("scroll", handleScroll);
    return () => scrollContainer.removeEventListener("scroll", handleScroll);
  }, [hasMore, isLoadingMore, onLoadMore, withPagination]);

  return (
    <div className="flex min-h-0  flex-1 flex-col gap-4 overflow-hidden">
      <div className="flex shrink-0 items-center justify-between gap-2">
        {withFilter && (
          <Input
            placeholder={filterPlaceholder}
            value={globalFilter}
            onChange={(e) => setGlobalFilter(e.target.value)}
            className="max-w-sm"
          />
        )}
        <div className="flex items-center gap-2 ">
          {withColumnVisibilityToggle && (
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Button className="w-50" variant="outline" size="sm">
                  Columns <ChevronDown className="ml-1 h-4 w-4" />
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent align="end">
                {table
                  .getAllColumns()
                  .filter((col) => col.getCanHide())
                  .map((col) => (
                    <DropdownMenuCheckboxItem
                      key={col.id}
                      className="capitalize"
                      checked={col.getIsVisible()}
                      onCheckedChange={(value) => col.toggleVisibility(!!value)}
                    >
                      {col.columnDef.header?.toString()}
                    </DropdownMenuCheckboxItem>
                  ))}
              </DropdownMenuContent>
            </DropdownMenu>
          )}
        </div>
        {toolbar}
      </div>

      <div className="flex flex-1 flex-col overflow-hidden rounded-md border">
        {/* Fixed Header */}
        <div className="shrink-0 overflow-x-auto">
          <Table>
            <TableHeader className="bg-background">
              {table.getHeaderGroups().map((headerGroup) => (
                <TableRow key={headerGroup.id}>
                  {headerGroup.headers.map((header) => (
                    <TableHead key={header.id} className="bg-background">
                      {header.isPlaceholder ? null : header.column.getCanSort() ? (
                        <button
                          className="inline-flex items-center gap-1.5 transition-colors hover:text-foreground"
                          onClick={header.column.getToggleSortingHandler()}
                        >
                          {flexRender(
                            header.column.columnDef.header,
                            header.getContext(),
                          )}
                          {header.column.getIsSorted() === "asc" ? (
                            <ChevronUp className="h-3.5 w-3.5" />
                          ) : header.column.getIsSorted() === "desc" ? (
                            <ChevronDown className="h-3.5 w-3.5" />
                          ) : (
                            <ChevronsUpDown className="h-3.5 w-3.5 text-muted-foreground" />
                          )}
                        </button>
                      ) : (
                        flexRender(
                          header.column.columnDef.header,
                          header.getContext(),
                        )
                      )}
                    </TableHead>
                  ))}
                </TableRow>
              ))}
            </TableHeader>
          </Table>
        </div>

        {/* Scrollable Body */}
        <div
          ref={scrollContainerRef}
          className="min-h-0 flex-1 overflow-auto scrollbar-thin scrollbar-track-background scrollbar-thumb-muted"
        >
          <Table>
            <TableBody>
              {table.getRowModel().rows.length > 0 ? (
                table.getRowModel().rows.map((row) => {
                  const rowId = String(
                    (row.original as { id?: string }).id ?? row.id,
                  );
                  const isExpanded = withViewDetails && expandedRowId === rowId;

                  return (
                    <React.Fragment key={row.id}>
                      <TableRow
                        data-state={row.getIsSelected() && "selected"}
                        className={
                          withViewDetails ? "cursor-pointer" : undefined
                        }
                        onClick={() => toggleRowDetails(row.original)}
                        onKeyDown={(event) => {
                          if (
                            withViewDetails &&
                            (event.key === "Enter" || event.key === " ")
                          ) {
                            event.preventDefault();
                            toggleRowDetails(row.original);
                          }
                        }}
                        role={withViewDetails ? "button" : undefined}
                        tabIndex={withViewDetails ? 0 : undefined}
                      >
                        {row.getVisibleCells().map((cell) => (
                          <TableCell className="text-left" key={cell.id}>
                            {flexRender(
                              cell.column.columnDef.cell,
                              cell.getContext(),
                            )}
                          </TableCell>
                        ))}
                      </TableRow>
                      {isExpanded && withViewDetails && (
                        <TableRow>
                          <TableCell
                            colSpan={allColumns.length}
                            className="p-0"
                          >
                            <DataTableDetails
                              row={row.original}
                              columns={
                                detailColumns as ColumnDef<TData, unknown>[]
                              }
                              isExpanded={isExpanded}
                              onToggle={() => toggleRowDetails(row.original)}
                              loading={loadingRowId === rowId}
                              error={detailErrorsById[rowId] ?? null}
                              details={detailRowsById[rowId] as TData[]}
                              title={detailTitle}
                            />
                          </TableCell>
                        </TableRow>
                      )}
                    </React.Fragment>
                  );
                })
              ) : (
                <TableRow>
                  <TableCell
                    colSpan={allColumns.length}
                    className="h-24 text-center text-muted-foreground"
                  >
                    No results.
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </div>

        {/* Fixed Footer */}
        {withPagination && (
          <div className="flex shrink-0 items-center justify-between border-t bg-background px-3 py-2">
            <p className="text-sm text-muted-foreground">
              {table.getFilteredSelectedRowModel().rows.length} of{" "}
              {table.getFilteredRowModel().rows.length} row(s) selected.
            </p>
            <div className="flex items-center gap-2">
              <Button
                variant="outline"
                size="sm"
                onClick={() => table.previousPage()}
                disabled={!table.getCanPreviousPage()}
              >
                Previous
              </Button>
              <span className="text-sm text-muted-foreground">
                Page {table.getState().pagination.pageIndex + 1} of{" "}
                {table.getPageCount()}
              </span>
              <Button
                variant="outline"
                size="sm"
                onClick={() => table.nextPage()}
                disabled={!table.getCanNextPage()}
              >
                Next
              </Button>
            </div>
          </div>
        )}
        {!withPagination && isLoadingMore && (
          <div className="flex shrink-0 items-center justify-center border-t bg-background px-3 py-2">
            <p className="text-sm text-muted-foreground">Loading more...</p>
          </div>
        )}
        {withTradeFooter && (
          <div className="flex shrink-0 items-center justify-between border-t bg-background px-3 py-2">
            <p className="text-sm text-muted-foreground">
              Trades: {table.getFilteredRowModel().rows.length}/{totalCount}
            </p>
          </div>
        )}
      </div>
    </div>
  );
}
