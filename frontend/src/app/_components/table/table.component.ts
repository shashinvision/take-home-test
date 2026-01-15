import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { MatTableModule } from "@angular/material/table";
import { MatButtonModule } from "@angular/material/button";
import { Loan } from "../../_models/loan";
import { LoanService } from "../../_services/loan.service";

@Component({
  selector: "app-table",
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule],
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
  constructor(private loanservice: LoanService) {}

  ngOnInit() {
    this.loanservice.getLoans().subscribe((loans) => {
      this.loans = loans;
    });
  }
}
