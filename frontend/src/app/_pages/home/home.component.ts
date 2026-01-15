import { Component } from "@angular/core";
import { CommonModule } from "@angular/common";
import { TableComponent } from "../../_components/table/table.component";

@Component({
  selector: "app-home",
  standalone: true,
  imports: [CommonModule, TableComponent],
  templateUrl: "./home.component.html",
  styleUrl: "./home.component.scss",
})
export class HomeComponent {}
