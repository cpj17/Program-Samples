const data = [
    {
      "group_id": "ADMIN",
      "form_id": "FORM1",
      "mod_id": "REPORTINGTOOLS"
    },
    {
      "group_id": "ADMIN",
      "form_id": "FORM2",
      "mod_id": "REPORTINGTOOLS"
    },
    {
      "group_id": "ADMIN",
      "form_id": "FORM3",
      "mod_id": "REPORTINGTOOLS"
    },
    {
      "group_id": "ADMIN44",
      "form_id": "FORM1",
      "mod_id": "REPORTINGTOOLS"
    }
  ];
  
  const filteredData = data.filter(item => item.form_id in { 'FORM1': 1, 'FORM3': 1 });
  
  console.log(filteredData);
  