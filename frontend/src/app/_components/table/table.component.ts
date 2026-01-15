import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { MatTableModule } from "@angular/material/table";
import { MatButtonModule } from "@angular/material/button";
import { MatIconModule } from "@angular/material/icon";
import { Loan } from "../../_models/loan";
import { LoanService } from "../../_services/loan.service";
import { MatDialog } from "@angular/material/dialog";
import { CreateLoanDialogComponent } from "../create-loan-dialog/create-loan-dialog.component";
import { CreatePaymentDialogComponent } from "../create-payment-dialog/create-payment-dialog.component";

@Component({
  selector: "app-table",
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule],
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
    "actions",
  ];

  constructor(
    private loanservice: LoanService,
    private dialog: MatDialog,
  ) {}

  ngOnInit() {
    this.loadLoans();
  }

  loadLoans() {
    this.loanservice.getLoans().subscribe((loans) => {
      this.loans = loans;
    });
  }

  openCreateLoan() {
    const dialogRef = this.dialog.open(CreateLoanDialogComponent, {
      width: "450px",
      disableClose: true,
    });

    dialogRef.afterClosed().subscribe((created) => {
      if (created) {
        this.loadLoans();
      }
    });
  }

  openPaymentDialog(loan: Loan) {
    // Solo permitir pagos si el balance es mayor a 0
    if (loan.currentBalance <= 0) {
      return;
    }

    const dialogRef = this.dialog.open(CreatePaymentDialogComponent, {
      width: "480px",
      disableClose: true,
      data: { loan },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadLoans();
      }
    });
  }
}
