export class Table extends React.Component {

    render() {
        let headingRow = [];
        let dataRows = [];

        let headings = this.props.headings;
        let properties = [];

        let headingCells = [];

        if (this.props.data != null && this.props.data != undefined && this.props.data != "undefined" && this.props.data.length > 0) {
            if (headings.length == 0) {
                for (const [key, value] of Object.entries(this.props.data[0])) {
                    headings.push(key);
                    properties.push(key);
                }
            }
            else {
                for (const [key, value] of Object.entries(this.props.data[0])) {
                    properties.push(key);
                }
            }

            for (let i = 0; i < headings.length; i++) {
                headingCells.push(
                    <th key={"hadingCell" + headings[i]}>{headings[i]}</th>
                );
            }

            headingRow.push(
                <thead key="thead">
                    <tr key="thead_tr">
                        {headingCells}
                    </tr>
                </thead>
            );

            for (let i = 0; i < this.props.data.length; i++) {
                let item = this.props.data[i];

                let cells = [];

                for (let j = 0; j < properties.length; j++) {
                    cells.push(
                        <td key={i + "_" + j}>{item[properties[j]]}</td>
                    );
                }

                dataRows.push(
                    <tr key={"row_" + i}>{cells}</tr>
                );
            }
        }

        return (
            <table className="table">
                {headingRow}
                <tbody>
                    {dataRows}
                </tbody>
            </table>
        );
    }

}

Table.defaultProps = { headings: [] };

//let data = [{ name: 'sonya', lastname: 'nadesan' }, { name: 'sofya', lastname: 'nadesan' }];
//let headings = ['name', 'lastname'];
//reactdom.render(<table data={data} headings={headings} />, document.getelementbyid('content'));