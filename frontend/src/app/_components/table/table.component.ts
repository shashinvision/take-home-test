import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { MatTableModule } from "@angular/material/table";
import { MatButtonModule } from "@angular/material/button";
import { Loan } from "../../_models/loan";
import { LoanService } from "../../_services/loan.service";
import { MatDialog } from "@angular/material/dialog";
import { CreateLoanDialogComponent } from "../create-loan-dialog/create-loan-dialog.component";

@Component({
  selector: "app-table",
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    CreateLoanDialogComponent,
  ],
  templateUrl: "./table.component.html",
  styleUrls: ["./table.component.scss"],
})
export class TableComponent implements OnInit {
  loans: Loan[] = [];

  displayedColumns: string[] = [
    "loanAmount",
    "currentBalance",
    "applicant",
    "status",
  ];
  constructor(
    private loanservice: LoanService,
    private dialog: MatDialog,
  ) {}

  ngOnInit() {
    this.loanservice.getLoans().subscribe((loans) => {
      this.loans = loans;
    });
  }

  openCreateLoan() {
    const dialogRef = this.dialog.open(CreateLoanDialogComponent, {
      width: "450px",
    });

    dialogRef.afterClosed().subscribe((created) => {
      if (created) {
        this.loanservice.getLoans().subscribe((loans) => {
          this.loans = loans;
        });
      }
    });
  }
}
