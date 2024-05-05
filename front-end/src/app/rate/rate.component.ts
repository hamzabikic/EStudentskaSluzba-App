import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {MojConfig} from "../MojConfig";
import {HttpClient} from "@angular/common/http";
import {MyAuthService} from "../Services/MyAuthService";

@Component({
  selector: 'app-rate',
  templateUrl: './rate.component.html',
  styleUrls: ['./rate.component.css']
})
export class RateComponent implements OnInit {
  studentId ="";
  ime="";
  prezime="";
  student:any;
  novaRata = false;
  godina="";
  redniBroj ="";
  obnova = false;
  datum="";
  lista:any;
  iznos ="";
  constructor(private route : ActivatedRoute, private http:HttpClient, private auth:MyAuthService) {
    this.studentId = route.snapshot.params["id"];
  }
  setujDatum() {
    this.datum = new Date().toISOString().split("T")[0];
  }

  async ngOnInit() {
    await this.ucitajStudenta();
    await this.ucitajUplate();
    this.setujDatum();
  }
  async ucitajStudenta() {
    this.student = await this.http.get(MojConfig.adresa_servera+"/Student/getById?id="+this.studentId).toPromise();
  }
  async ucitajUplate () {
    this.lista = await this.http.get(MojConfig.adresa_servera + "/Uplate/getRateById?studentId="+ this.studentId)
      .toPromise();
  }
  async posaljiUplatu() {
    let obj= {
      godina : this.godina,
      redniBroj : this.redniBroj,
      obnova: this.obnova,
      datum: this.datum,
      studentId : this.studentId,
      referentId: this.auth.getId(),
      iznos:this.iznos
    };
    let res = await this.http.post(MojConfig.adresa_servera+"/Uplate/addRata",obj).toPromise();
    if(res == true) {
      alert("Uspjesno slanje uplate!");
      await this.ucitajUplate();
      this.novaRata =!this.novaRata;
      return;
    }
    alert("Neuspjesno slanje!");
  }
  async obrisiUplatu(id:number) {
    let res =await this.http.delete(MojConfig.adresa_servera +"/Uplate/deleteRata?rataId="+id).toPromise();
    if(res == true) {
      alert("Uspjesno brisanje!");
      await this.ucitajUplate();
      return;
    }
    alert("Neuspjesno brisanje!");
  }

}
