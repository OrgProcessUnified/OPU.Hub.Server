<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Order Point</title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.0/jquery.min.js"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            var sampleInputObject = {
                DynamicReport: {
                    DynamicReportId: 3,
                    Details: [
                        { "DetailFieldNo": 17, "N1": 120, "N2": 5 },
                        { "DetailFieldNo": 19, "N1": 130, "N2": 4 },
                        { "DetailFieldNo": 17, "N1": 110, "N2": 7 }
                    ]
                },
                DynamicReportType: {
                    DynamicReportTypeId: 17,
                    DynamicReportTypeName: "DSR",
                    ValidationCodeWinform: `

                var orderLines =  jQuery.grep(data.DynamicReport.Details, function( d, i ) {
					return ( d.DetailFieldNo == 17);
				});

				$.each(orderLines, function( i, o ) {
					o.N3 = o.N1 * o.N2;
				});

				var orderValue = 0;
				$.each(orderLines, function (i, o) {
					orderValue = orderValue + o.N3;
				});

				if (orderValue > 1000)
					data.ValidationResults.push({
						"SlNo" : 1,
						"Message" : "Total value of order cannot exceed 1000"
					});

				return data;


						`
                },
                ValidationResults: [],
                IsWorkflowRequested: false
            };

            var button_Test = jQuery('#test_Button');

            button_Test.bind('click', function () {
                var output = doValidateTest();
            })
        });

        function doValidateTest() {
            alert("Welcome to Order Point!");
            testGroupBy();
        }

        function doValidate(data) {
            var validationCodeWinform = data.DynamicReportType.ValidationCodeWinform.replace(/&quot;/g, '"');
            var func_Validate = new Function('data', validationCodeWinform);
            var result = func_Validate(data);
            return JSON.stringify(result);
        }

        function getLookupValue(lookups, lookupId) {
            var lookup = lookups.filter(function (i) {
                return (i.LookupId === lookupId);
            });

            if (lookup.length > 0)
                return lookup[0].LookupValue;
            else
                return '';
        }

        function getOrderBucketName(orderBuckets, orderBucketId) {
            var orderBucket = orderBuckets.filter(function (i) {
                return (i.OrderBucketId === orderBucketId);
            });

            if (orderBucket.length > 0)
                return orderBucket[0].OrderBucketName;
            else
                return '';
        }

        function getKeyObject(item, attributes) {
            let keyObject = {};
            attributes.forEach(function (a) {
                keyObject[a] = item[a];
            });

            return keyObject;
        }

        function getGroupByDuplicates(array, attributes) {
            var groupResult = groupBy(array, attributes);

            return groupResult.filter(function (i) {
                return (i.Count > 1);
            });
        }

        function groupBy(array, attributes) {
            let groups = {};
            array.forEach(function (o) {
                let keyObject = getKeyObject(o, attributes);

                let group = JSON.stringify(keyObject);
                if (groups[group])
                    groups[group].Count = groups[group].Count + 1;
                else {
                    keyObject.Count = 1;
                    groups[group] = keyObject;
                }
            });

            return Object.keys(groups).map(function (group) {
                return groups[group];
            })
        }

        function populateUserFilterByRole(data, filterKey, detailFieldNo, roleLookupNo, ){
	        var roleLookupField = `L${roleLookupNo}`;
	        var observations =  jQuery.grep(data.DynamicReport.Details, function( d, i ) {
		        return (( d.DetailFieldNo == detailFieldNo) && (d.LocalId == data.DynamicReport.CurrentDetailLocalId));
	        });

	        data.Filters[filterKey] = [];
	        if (observations.length > 0){
		        var observation = observations[0];
		
		        var roleLookupId = observation[roleLookupField];
		
		        if(roleLookupId > 0){
			        var roleLookups = jQuery.grep(data.Lookups, function( l, i ) {
					        return ( l.LookupId == roleLookupId);
				        });
				
			        if (roleLookups.length > 0){
				        var roleId = parseInt(roleLookups[0].LookupKey);
				
				        if (roleId > 0){
					        var roles = jQuery.grep(data.AllRoles, function( r, i ) {
							        return ( r.RoleId == roleId);
						        });
						
					        if (roles.length > 0){
						        data.Filters[filterKey] = roles[0].Users;
					        }					
				        }
			        }
		        }
	        }
        }

        function getCurrentDetail(detailFieldNo){
	        var currentDetails =  jQuery.grep(data.DynamicReport.Details, function( d, i ) {
			        return (( d.DetailFieldNo == detailFieldNo) && (d.LocalId == data.DynamicReport.CurrentDetailLocalId));
		        });

	        return (currentDetails.length > 0) ?  currentDetails[0] : null;
        }

        function durationInMinutes(date1, date2, ignoreDate, allowNegative){
	        if (ignoreDate){
		         var duration = ((date2.getHours() * 60) + (date2.getMinutes())) - ((date1.getHours() * 60) + (date1.getMinutes()));
		         return allowNegative ? duration : (duration > 0 ? duration : 0);
	        }else{
		        return parseInt((date2 - date1) / 1000 / 60);
	        }
        }
    </script>

    <script type="text/javascript">
        function testGroupBy() {
            let arr = [{ "shape": "square", "color": "red", "instances": 1 },
                { "shape": "square", "color": "red", "instances": 1 },
                { "shape": "circle", "color": "blue", "instances": 0 },
                { "shape": "square", "color": "blue", "instances": 4 },
                { "shape": "circle", "color": "red", "instances": 1 },
                { "shape": "circle", "color": "red", "instances": 0 },
                { "shape": "square", "color": "blue", "instances": 5 },
                { "shape": "square", "color": "red", "instances": 1 }];

            let result = groupBy(arr, ["shape", "color"]);

            alert(JSON.stringify(result));
        }
    </script>


</head>
<body>
    <button id="test_Button">Welcome to Order Point!</button>
</body>
</html>
