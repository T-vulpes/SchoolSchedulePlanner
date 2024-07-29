<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="courseschedule.main" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Class Schedule Creator</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f0f0f0;
            color: #333;
            margin: 0;
            padding: 0;
        }
        form {
            background-color: #fff;
            margin: 50px auto;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            max-width: 600px;
        }
        h2 {
            text-align: center;
            color: #333;
        }
        label {
            display: block;
            margin-top: 10px;
        }
        select, input[type="text"], textarea {
            width: calc(100% - 22px);
            padding: 10px;
            margin-top: 5px;
            border: 1px solid #ccc;
            border-radius: 5px;
        }
        textarea {
            height: 100px;
        }
        button {
            display: inline-block;
            padding: 10px 20px;
            margin-top: 20px;
            margin-right: 10px;
            border: none;
            background-color: #007bff;
            color: #fff;
            border-radius: 5px;
            cursor: pointer;
            transition: background-color 0.3s;
        }
        button:hover {
            background-color: #0056b3;
        }
        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }
        th, td {
            border: 1px solid #ccc;
            padding: 10px;
            text-align: center;
        }
        th {
            background-color: #007bff;
            color: #fff;
        }
        td {
            background-color: #f9f9f9;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Create Your Class Schedule</h2>
            <label for="educationLevel">Education Level:</label>
            <asp:DropDownList ID="EducationLevel" runat="server">
                <asp:ListItem Value="elementary">Elementary School</asp:ListItem>
                <asp:ListItem Value="middle">Middle School</asp:ListItem>
                <asp:ListItem Value="high">High School</asp:ListItem>
            </asp:DropDownList>
            <br />
            
            <label for="year">Year:</label>
            <asp:TextBox ID="Year" runat="server"></asp:TextBox>
            <br />
            
            <label for="courses">Courses (one per line with hours, e.g., Math - 5):</label>
            <asp:TextBox ID="Courses" runat="server" TextMode="MultiLine" Rows="5" Columns="30"></asp:TextBox>
            <br />
            
            <button type="button" id="SubmitButton" runat="server" onserverclick="SubmitButton_Click">Submit</button>
            <button type="button" id="ClearButton" runat="server" onserverclick="ClearButton_Click">Clear</button>
            
            <br />
            <asp:Panel ID="SchedulePanel" runat="server" Visible="false">
                <h2>Class Schedule</h2>
                <asp:Table ID="ScheduleTable" runat="server" BorderWidth="1" CellPadding="5" />
                <br />
                <button type="button" id="DownloadButton" runat="server" onserverclick="DownloadButton_Click">Download Schedule</button>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
